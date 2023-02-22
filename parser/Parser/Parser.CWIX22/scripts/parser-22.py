from bs4 import BeautifulSoup
from enum import Enum
import os
import json
from fuzzywuzzy import fuzz


class ItemEnum(Enum):
    TC = 1
    TT = 2
    ORG = 3
    FA = 4
    CC = 5
    OB = 6


def parse_directory(import_path: str, export_path: str, item: ItemEnum, cycle: str):
    def _TT(file_path) -> bool:
        """
            <summary>
                Parses all TTs in html files in a directory and exports them as json files.
            </summary>
            <param name="import_path">Path to directory</param>
            <returns>True if all files were parsed successfully, False otherwise</returns>
        """
        try:
            to_export = {
                "id": "",
                "cycle": "",
                "status": "",
                "purpose": "",
                "precondition": "",
                "steps": [],
                "successcriteria": "",
                "limitedsuccesscriteria": "",
                "interopissuecriteria": "",
                "standards": []
            }

            html = BeautifulSoup(
                open(file_path, encoding="utf8"), "html.parser")

            # Wait until file loaded successfully
            while html.find("h1", {"id": "firstHeading"}) is None:
                html = BeautifulSoup(
                    open(file_path, encoding="utf8"), "html.parser")

            to_export["id"] = html.find(
                "h1", {"id": "firstHeading"}).text.split("-")[-1]
            to_export["cycle"] = cycle

            # Find all tables with class wikitable
            wikitables = html.find_all("table", {"class": "wikitable"})

            # wikitable[0] - Table that contains standards
            # wikitable[1] - Table that contains other info

            # Standards
            standards = wikitables[0].find(
                "th", string="Standards").find_next_sibling().find_all("a")
            to_export["standards"] = list(
                map(lambda x: x.text.strip().replace("\n", ''), standards))

            # Status
            to_export["status"] = html.find("div", {"class": "grid"}).find("div", {"class": "panel"}).find(
                "div", {"class": "banner-bottom-center"}).text.strip().replace("\n", '')

            # Purpose
            to_export["purpose"] = wikitables[1].find(
                "th", string="Purpose").find_next_sibling().getText().strip().replace("\n", '')
            to_export["precondition"] = wikitables[1].find(
                "th", string="Pre-condition").find_next_sibling().text.strip().replace("\n", '')

            # Steps
            steps_elems = wikitables[1].find(
                "th", string="Steps").find_next_sibling().find_all("tr")

            # Remove header
            steps_elems = steps_elems[1:]

            # Parse steps
            for step in steps_elems:
                to_export["steps"].append({
                    "order": step.find_all("td")[0].text.strip().replace("\n", ''),
                    "description": step.find_all("td")[1].text.strip().replace("\n", ''),
                    "expectedresult": step.find_all("td")[2].text.strip().replace("\n", '')
                })

            # Get criteria table
            criteria_table = wikitables[1].find(
                "table", {"class": "properties"})

            # Success Criteria
            to_export["successcriteria"] = criteria_table.find(
                "th", string="Success").find_next_sibling().text.strip().replace("\n", '')

            # Limited Success Criteria
            to_export["limitedsuccesscriteria"] = criteria_table.find(
                "th", string="Limited Success").find_next_sibling().text.strip().replace("\n", '')

            # Interoperability Issue Criteria
            to_export["interopissuecriteria"] = criteria_table.find(
                "th", string="Interoperability Issue").find_next_sibling().text.strip().replace("\n", '')

            # Export file as json
            with open(f"{os.path.join(export_path, os.path.basename(file_path))}.json", "w") as f:
                json.dump(to_export, f, indent=4)

            return True

        except Exception as e:
            no_article_elem = html.find("div", {"class": "noarticletext"})

            if no_article_elem is not None:
                print(f"Article {file_path} does not exist.")
            else:
                print(
                    f"Exception occured while parsing {file_path}.", e, type(e))

            return False

    def _OB(file_path) -> bool:
        """
            <summary>
                Parses all OBs in html files in a directory and exports them as json files.
            </summary>
            <param name="import_path">Path to directory</param>
            <returns>True if all files were parsed successfully, False otherwise</returns>
        """
        try:
            to_export = {
                "id": "",
                "cycle": "",
                "title": "",
                "relevantstandards": [],
                "scope": {},
                "objectiveresult": "",
                "summary": "",
                "recommentation": "",
                "description": ""
            }

            html = BeautifulSoup(
                open(file_path, encoding="utf8"), "html.parser")

            # Wait until file loaded successfully
            while html.find("h1", {"id": "firstHeading"}) is None:
                html = BeautifulSoup(
                    open(file_path, encoding="utf8"), "html.parser")

            to_export["cycle"] = cycle

            # Objective
            to_export["id"] = html.find("div", {"class": "panel"}).find("div", string="Objective").find_next_sibling().find(
                "div", {"class": "banner-center"}).text.split("-")[-1]

            # Description
            to_export["description"] = html.find(
                "th", string="Description").find_next_sibling().text.strip("\n").replace("\n", '')

            # Title
            to_export["title"] = html.find(
                "th", string="Title").find_next_sibling().text.strip("\n").replace("\n", '')

            # Relevant Standards
            to_export["relevantstandards"] = list(map(lambda x: x.text.strip("\n").replace(
                "\n", ''), html.find("th", string="Relevant Standards").find_next_sibling().find_all("a")))

            # Scope
            scope_table = html.find(
                "th", string="Scope").find_next_sibling().find("table")
            scopes = scope_table.find_all("tr")
            to_export["scope"] = {
                "exploration": scopes[1].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                "experimentation": scopes[1].find_all("td")[1].text.strip("\n").replace("\n", '') == "X",
                "examination": scopes[1].find_all("td")[2].text.strip("\n").replace("\n", '') == "X",
                "exercise": scopes[1].find_all("td")[3].text.strip("\n").replace("\n", '') == "X",
            }

            # Objective Result
            objective_result = html.find(
                "th", string="Objective Result")
            if objective_result is not None:
                to_export["objectiveresult"] = objective_result.find_next_sibling(
                ).text.strip("\n").replace("\n", '')
            else:
                to_export["objectiveresult"] = ""

            # Summary
            to_export["summary"] = html.find(
                "div", string="Summary").find_next_sibling().text.strip("\n").replace("\n", '')

            # Recommendation
            to_export["recommentation"] = html.find(
                "div", string="Recommendations").find_next_sibling().text.strip("\n").replace("\n", '')

            # Export file as json
            with open(f"{os.path.join(export_path, os.path.basename(file_path))}.json", "w") as f:
                json.dump(to_export, f, indent=4)

            return True

        except Exception as e:
            no_article_elem = html.find("div", {"class": "noarticletext"})

            if no_article_elem is not None:
                print(f"Article {file_path} does not exist.")
            else:
                print(
                    f"Exception occured while parsing {file_path}.", e, type(e))

            return False

    def _TC(html):
        pass

    def _ORG(html):
        pass

    def _FA(html):
        pass

    def _CC(html):
        pass

    # Iterate through each html file in directory
    successfull = 0
    failed = 0
    total = 0
    for file in os.listdir(import_path):
        if file.endswith(".html"):
            file_path = os.path.join(import_path, file)
            if item == ItemEnum.TC:
                _TC(file_path)
            elif item == ItemEnum.TT:
                if (_TT(file_path)):
                    successfull += 1
                else:
                    failed += 1
            elif item == ItemEnum.ORG:
                _ORG(file_path)
            elif item == ItemEnum.FA:
                _FA(file_path)
            elif item == ItemEnum.CC:
                _CC(file_path)
            elif item == ItemEnum.OB:
                if (_OB(file_path)):
                    successfull += 1
                else:
                    failed += 1
            total += 1

    print(f"Finished parsing {item.name}. Total: {total}")
    print(f"Successfull: {successfull}")
    print(f"Failed: {failed}")


def jaccard(value_1: str, value_2: str) -> float:
    """
        <summary>
            Gives the jaccard similarity between two values.
        </summary>
        <param name="value_1">First value</param>
        <param name="value_2">Second value</param>
        <returns>Similarity value</returns>
    """

    value_1 = set(value_1.split())
    value_2 = set(value_2.split())

    return len(value_1.intersection(value_2)) / len(value_1.union(value_2))


def print_similarities(list_1: list, list_2: list):
    """
        <summary>
            Prints the similarity between two lists.
        </summary>
        <param name="list_1">First list</param>
        <param name="list_2">Second list</param>
    """

    for value_1 in list_1:
        max_similarity = 0
        similar_object = None
        for value_2 in list_2:
            desc_similarity = fuzz.ratio(value_1["description"], value_2["description"])
            title_similarity = fuzz.ratio(value_1["title"], value_2["title"])

            if desc_similarity + title_similarity > max_similarity:
                max_similarity = desc_similarity + title_similarity
                similar_object = value_2
        
        print(f"\tOB_1: {value_1['description']}\n\tOB_2: {similar_object['description']}")
        print(f"\tOB_1: {value_1['title']}\n\tOB_2: {similar_object['title']}")
        print("---------------------------------")


if __name__ == "__main__":

    # import_path = "/Users/teodorcazamir/Desktop/frostbyte/data/normalized/cwix21/ob"
    # ob_1 = []
    # for file in os.listdir(import_path):
    #     if file.endswith(".json"):
    #         file_path = os.path.join(import_path, file)
    #         with open(file_path, "r") as f:
    #             data = json.load(f)

    #             ob_1.append({
    #                 "description": data["description"],
    #                 "title": data["title"],
    #             })

    # import_path = "/Users/teodorcazamir/Desktop/frostbyte/data/normalized/cwix22/ob"
    # ob_2 = []
    # for file in os.listdir(import_path):
    #     if file.endswith(".json"):
    #         file_path = os.path.join(import_path, file)
    #         with open(file_path, "r") as f:
    #             data = json.load(f)

    #             ob_2.append({
    #                 "description": data["description"],
    #                 "title": data["title"],
    #             })
    
    # print_similarities(ob_1, ob_2)
    


    import_path = "/Users/teodorcazamir/Desktop/frostbyte/data/raw/cwix21/tt"
    export_path = "/Users/teodorcazamir/Desktop/frostbyte/data/normalized/cwix21/tt"
    parse_directory(
        import_path=import_path,
        export_path=export_path,
        item=ItemEnum.TT,
        cycle="CWIX2021")

    import_path = "/Users/teodorcazamir/Desktop/frostbyte/data/raw/cwix21/ob"
    export_path = "/Users/teodorcazamir/Desktop/frostbyte/data/normalized/cwix21/ob"
    parse_directory(
        import_path=import_path,
        export_path=export_path,
        item=ItemEnum.OB,
        cycle="CWIX2021")
    
    import_path = "/Users/teodorcazamir/Desktop/frostbyte/data/raw/cwix22/tt"
    export_path = "/Users/teodorcazamir/Desktop/frostbyte/data/normalized/cwix22/tt"
    parse_directory(
        import_path=import_path,
        export_path=export_path,
        item=ItemEnum.TT,
        cycle="CWIX2022")

    import_path = "/Users/teodorcazamir/Desktop/frostbyte/data/raw/cwix22/ob"
    export_path = "/Users/teodorcazamir/Desktop/frostbyte/data/normalized/cwix22/ob"
    parse_directory(
        import_path=import_path,
        export_path=export_path,
        item=ItemEnum.OB,
        cycle="CWIX2022")
