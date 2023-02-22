from bs4 import BeautifulSoup
import os
import json
import re
from fuzzywuzzy import fuzz


class TT:
    def __init__(self, id, cycle, status, purpose, precondition, steps, successcriteria, limitedsuccesscriteria, interopissuecriteria, standards, diffusion=None, timeline=[], _file_path=None):
        self.id = id
        self.cycle = cycle
        self.status = status
        self.purpose = purpose
        self.precondition = precondition
        self.steps = steps
        self.successcriteria = successcriteria
        self.limitedsuccesscriteria = limitedsuccesscriteria
        self.interopissuecriteria = interopissuecriteria
        self.standards = standards

        self.diffusion = diffusion
        self.timeline = timeline

        self._file_path = _file_path

    def __str__(self) -> str:
        return f"\t{self.id} [{self.cycle}]\n\t[PURPOSE]: {self.purpose}\n\t[STEPS]: {list(map(lambda step: step['description'], self.steps))}"

    @staticmethod
    def from_html_file(file_path, cycle):
        """Create a TT object from a HTML file"""
        try:

            html = BeautifulSoup(
                open(file_path, encoding="utf8"), "html.parser")

            # Wait until file loaded successfully
            while html.find("h1", {"id": "firstHeading"}) is None:
                html = BeautifulSoup(
                    open(file_path, encoding="utf8"), "html.parser")

            id = html.find(
                "h1", {"id": "firstHeading"}).text.strip().replace("\n", '')

            # Find all tables with class wikitable
            wikitables = html.find_all("table", {"class": "wikitable"})

            # wikitable[0] - Table that contains standards
            # wikitable[1] - Table that contains other info

            # Standards
            standards = wikitables[0].find(
                "th", string="Standards").find_next_sibling().find_all("a")
            standards = list(
                map(lambda x: x.text.strip().replace("\n", ''), standards))

            # Status
            status = html.find("div", {"class": "grid"}).find("div", {"class": "panel"}).find(
                "div", {"class": "banner-bottom-center"}).text.strip().replace("\n", '')

            # Purpose
            purpose = wikitables[1].find(
                "th", string="Purpose").find_next_sibling().getText().strip().replace("\n", '')
            precondition = wikitables[1].find(
                "th", string="Pre-condition").find_next_sibling().text.strip().replace("\n", '')

            # Steps
            steps_elems = wikitables[1].find(
                "th", string="Steps").find_next_sibling().find_all("tr")

            # Remove header
            steps_elems = steps_elems[1:]

            steps = []
            # Parse steps
            for index, step in enumerate(steps_elems):
                cells = step.find_all("td")

                order = index
                description = cells[1].text.strip().replace(
                    "\n", '') if len(cells) > 1 else ''
                expectedresult = cells[2].text.strip().replace(
                    "\n", '') if len(cells) > 2 else ''

                steps.append({
                    "order": order,
                    "description": description,
                    "expectedresult": expectedresult
                })

            # Get criteria table
            criteria_table = html.find(
                "table", {"class": "properties"})

            if criteria_table:
                # Success Criteria
                successcriteria = criteria_table.find(
                    "th", string="Success").find_next_sibling().text.strip().replace("\n", '')

                # Limited Success Criteria
                limitedsuccesscriteria = criteria_table.find(
                    "th", string="Limited Success").find_next_sibling().text.strip().replace("\n", '')

                # Interoperability Issue Criteria
                interopissuecriteria = criteria_table.find(
                    "th", string="Interoperability Issue").find_next_sibling().text.strip().replace("\n", '')
            else:
                criteria_table = html.find(
                    "span", string="Validation criteria").find_parent("th").find_next_sibling()

                criteria_text = criteria_table.text.strip().replace("\n", '')
                non_alphanumeric_char = "[^a-zA-Z\d\s: ]*"
                tokens = [rf"{non_alphanumeric_char}SUCCESS{non_alphanumeric_char}", 
                          rf"{non_alphanumeric_char}LIMITED SUCCESS{non_alphanumeric_char}", 
                          rf"{non_alphanumeric_char}PARTIAL SUCCESS{non_alphanumeric_char}",
                            rf"{non_alphanumeric_char}INTEROPERABILITY ISSUE{non_alphanumeric_char}"]

                # Split using regex
                criteria = re.split(r'|'.join(tokens), criteria_text)

                # Remove empty strings
                criteria = list(filter(None, criteria))





                # Remove whitespace
                criteria = list(map(lambda x: x.strip(), criteria))

                if len(criteria) > 3:
                    # Remove first element
                    criteria = criteria[1:]

                successcriteria = criteria[0].lstrip(": .-") if len(criteria) > 0 else ''
                limitedsuccesscriteria = criteria[1].lstrip(": .-") if len(criteria) > 1 else ''
                interopissuecriteria = criteria[2].lstrip(": .-") if len(criteria) > 2 else ''

            return TT(
                id=id,
                cycle=cycle,
                status=status,
                purpose=purpose,
                precondition=precondition,
                steps=steps,
                successcriteria=successcriteria,
                limitedsuccesscriteria=limitedsuccesscriteria,
                interopissuecriteria=interopissuecriteria,
                standards=standards,
                _file_path=file_path
            )

        except Exception as e:
            no_article_elem = html.find("div", {"class": "noarticletext"})

            if no_article_elem is not None:
                print(f"Article {file_path} does not exist.")
            else:
                print(
                    f"Exception occured while parsing {file_path}.", e, type(e))

            return None

    @staticmethod
    def from_json_file(json_file):
        """Create a TT object from a JSON file"""

        try:

            with open(json_file, "r") as f:
                json_data = json.load(f)

            return TT(
                id=json_data["id"],
                cycle=json_data["cycle"],
                status=json_data["status"],
                purpose=json_data["purpose"],
                precondition=json_data["precondition"],
                steps=json_data["steps"],
                successcriteria=json_data["successcriteria"],
                limitedsuccesscriteria=json_data["limitedsuccesscriteria"],
                interopissuecriteria=json_data["interopissuecriteria"],
                standards=json_data["standards"],
                diffusion=json_data["diffusion"],
                timeline=json_data["timeline"],
                _file_path=json_file
            )
        except Exception as e:
            print(
                f"Exception occured while parsing {json_file}.", e, type(e))

            return None

    def export_to_json(self, path):
        """Export a TT object to a JSON file"""

        with open(f"{os.path.join(path, self.id)}.json", "w") as f:
            json.dump({
                "id": self.id,
                "cycle": self.cycle,
                "status": self.status,
                "purpose": self.purpose,
                "precondition": self.precondition,
                "steps": self.steps,
                "successcriteria": self.successcriteria,
                "limitedsuccesscriteria": self.limitedsuccesscriteria,
                "interopissuecriteria": self.interopissuecriteria,
                "standards": self.standards,
                "diffusion": self.diffusion,
                "timeline": self.timeline
            }, f, indent=4)

    def get_similarity(self, other):
        """Compute the similarity between two TT objects"""

        if not isinstance(other, TT):
            raise TypeError("Comparer must be of type TT")

        # Compute purpose similarity
        purpose_similarity = fuzz.ratio(self.purpose, other.purpose)

        # Compute steps similarity
        if len(self.steps) != len(other.steps):
            return 0

        steps_similarity = 0
        if len(self.steps) == 0:
            steps_similarity = 1
        else:
            for i in range(len(self.steps)):
                steps_similarity += fuzz.ratio(
                    self.steps[i]["description"], other.steps[i]["description"])
            steps_similarity /= len(self.steps)

        purpose_ponder, steps_ponder = 0.6, 0.4

        return purpose_similarity * purpose_ponder + steps_similarity * steps_ponder

    def get_most_similar(self, other: list, count=1):
        """Get the most similar TT objects from a list of TT objects"""

        if not isinstance(other, list):
            raise TypeError("Comparer must be of type list")

        similarities = []
        for tt in other:
            similarities.append((tt, self.get_similarity(tt)))

        similarities.sort(key=lambda x: x[1], reverse=True)

        return similarities[:count]

    def compute_diffusion(self, cycle: list):
        """Computes most similar TT object in the current cycle"""

        if self.diffusion is not None:
            # I am a child
            return
        else:
            most_similar = self.get_most_similar(
                [tt for tt in cycle if tt.id != self.id], count=len(cycle) - 1)

            for similar in most_similar:
                similar_obj = similar[0]
                similarity = similar[1]

                if similarity > 95:
                    similar_obj.diffusion = {
                        "id": self.id,
                        "similarity": similarity
                    }

                    print(
                        f"[DIFFUSION][{self.cycle}] {self.id} is a parent of {similar_obj.id} is [SIMILARITY - {similarity:.2f}]")

                    # Save change to JSON
                    similar_obj.export_to_json(
                        os.path.dirname(similar_obj._file_path))

    def compute_timeline(self, cycles: list):
        """Computes most similar TT object in each cycle"""

        # If I am a child, I don't compute my timeline
        if self.diffusion is not None:
            return

        for cycle in cycles:
            cycle_number = cycle[0].cycle

            if len(self.timeline) > 0 and cycle_number in [x["cycle"] for x in self.timeline]:
                # I already computed this cycle
                continue

            # Get similarities
            similarities = self.get_most_similar(cycle, count=len(cycle))

            # Filter to only take parents
            similarities = list(filter(lambda x: x[0].diffusion is None and self.cycle not in [
                                y['id'] for y in x[0].timeline], similarities))

            # Sort similarities
            similarities.sort(key=lambda x: x[1], reverse=True)

            for similar in similarities:
                if len(similar[0].timeline) > 0 and self.cycle in [x["cycle"] for x in similar[0].timeline]:
                    # I already computed this cycle
                    continue
                else:
                    if similar[1] > 95:

                        self.timeline.append({
                            "id": similar[0].id,
                            "cycle": similar[0].cycle,
                            "similarity": similar[1]
                        })

                        similar[0].timeline.append({
                            "id": self.id,
                            "cycle": self.cycle,
                            "similarity": similar[1]
                        })

                        # Save change to JSON
                        similar[0].export_to_json(
                            os.path.dirname(similar[0]._file_path))

                        print(
                            f"[TIMELINE][{self.cycle}] {self.id} is similar with {similar[0].id} in cycle {similar[0].cycle} [SIMILARITY - {similar[1]:.2f}]")
                        break

        # Save change to JSON
        self.export_to_json(os.path.dirname(self._file_path))
