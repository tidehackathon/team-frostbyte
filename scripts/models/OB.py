from bs4 import BeautifulSoup
import os
import json
from typing import List
from fuzzywuzzy import fuzz

ratio = 0.95

def similarity(str1, str2, ratio=ratio):
    return fuzz.ratio(str1, str2) / 100 >= ratio

class OB:
    def __init__(self, id, cycle, title, relevantstandards, scope, objectiveresult, summary, recommendation, description,compability,capabilitiesids=[]):
        self.id = id
        self.cycle = cycle
        self.title = title
        self.relevantstandards = relevantstandards
        self.scope = scope
        self.objectiveresult = objectiveresult
        self.summary = summary
        self.recommendation = recommendation
        self.description = description
        self.compability=compability
        self.capabilitiesids = capabilitiesids




    @staticmethod
    def from_html_file(file_path, cycle):
        """Create a OB object from a HTML file"""
        try:

            html = BeautifulSoup(
                open(file_path, encoding="utf8"), "html.parser")

            # Wait until file loaded successfully
            while html.find("h1", {"id": "firstHeading"}) is None:
                html = BeautifulSoup(
                    open(file_path, encoding="utf8"), "html.parser")
                
            # Objective
            id = html.find("div", {"class": "panel"}).find("div", string="Objective").find_next_sibling().find(
                "div", {"class": "banner-center"}).text.split("-")[-1]
            
            # Description
            description = html.find(
                "th", string="Description").find_next_sibling().text.strip("\n").replace("\n", '')

            title=''
            try:
                # Title
                title = html.find(
                    "th", string="Title").find_next_sibling().text.strip("\n").replace("\n", '')
            except:
                pass

            relevantstandards = []

            try:
               # Relevant Standards
                relevantstandards = list(map(lambda x: x.text.strip("\n").replace(
                    "\n", ''), html.find("th", string="Relevant Standards").find_next_sibling().find_all("a")))
            except:
                pass

            
            # Scope
            scope_table = html.find(
                "th", string="Scope")
            if(scope_table is None):
                scope_table = html.find(
                    "div", string="Scope")
            scope_table = scope_table.find_next_sibling().find("table")

            scopes = scope_table.find_all("tr")

            try:
                scope = {
                    "exploration": scopes[1].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "experimentation": scopes[1].find_all("td")[1].text.strip("\n").replace("\n", '') == "X",
                    "examination": scopes[1].find_all("td")[2].text.strip("\n").replace("\n", '') == "X",
                    "exercise": scopes[1].find_all("td")[3].text.strip("\n").replace("\n", '') == "X",
                }
            except:
                scope = {
                    "exploration": scopes[0].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "experimentation": scopes[1].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "examination": scopes[2].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "exercise": scopes[3].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                }
            
            # Objective Result
            objective_result = html.find(
                "th", string="Objective Result")
            if objective_result is not None:
                objectiveresult = objective_result.find_next_sibling(
                ).text.strip("\n").replace("\n", '')
            else:
                objectiveresult = ""

            # Summary
            summary = html.find(
                "div", string="Summary").find_next_sibling().text.strip("\n").replace("\n", '')

            # Recommendation
            recommentation = html.find(
                "div", string="Recommendations").find_next_sibling().text.strip("\n").replace("\n", '')
            
            div=html.find("div",string="Signed-up Capability Configurations")

            if(div is None):
                div=html.find("div",string="Planned Capability Configurations")
                
            trs=div.find_next_sibling().find("table").find_all("tr")[1:]


            capabilities=[x.find_all("td")[1].text.split()[0].strip() for x in trs]

            return OB(id, cycle, title, relevantstandards, scope, objectiveresult, summary, recommentation, description,[],capabilities)

        except Exception as e:
            no_article_elem = html.find("div", {"class": "noarticletext"})

            if no_article_elem is not None:
                print(f"Article {file_path} does not exist.")
            else:
                print(
                    f"Exception occured while parsing {file_path}.", e, type(e))

            return None
    
    @staticmethod
    def from_html_file2019(file_path, cycle):
        """Create a OB object from a HTML file"""
        try:

            html = BeautifulSoup(
                open(file_path, encoding="utf8"), "html.parser")

            # Wait until file loaded successfully
            while html.find("h1", {"id": "firstHeading"}) is None:
                html = BeautifulSoup(
                    open(file_path, encoding="utf8"), "html.parser")
                
            # Objective
            id = html.find("h1",id="firstHeading").text.strip()

            print(id)

            # Description
            description = html.find(
                "th", string="Description").find_next_sibling().text.strip()

            title=''
            try:
                # Title
                title = html.find('div',class_="banner-center").text.strip()
            except:
                pass

            relevantstandards = []

            try:
               # Relevant Standards
                relevantstandards = list(map(lambda x: x.text.strip("\n").replace(
                    "\n", ''), html.find("th", string="Relevant Standards").find_next_sibling().find_all("a")))
            except:
                pass

            
            # Scope
            scope_table = html.find(
                "th", string="Scope")
            if(scope_table is None):
                scope_table = html.find(
                    "div", string="Scope")
            scope_table = scope_table.find_next_sibling().find("table")

            scopes = scope_table.find_all("tr")

            try:
                scope = {
                    "exploration": scopes[1].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "experimentation": scopes[1].find_all("td")[1].text.strip("\n").replace("\n", '') == "X",
                    "examination": scopes[1].find_all("td")[2].text.strip("\n").replace("\n", '') == "X",
                    "exercise": scopes[1].find_all("td")[3].text.strip("\n").replace("\n", '') == "X",
                }
            except:
                scope = {
                    "exploration": scopes[0].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "experimentation": scopes[1].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "examination": scopes[2].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                    "exercise": scopes[3].find_all("td")[0].text.strip("\n").replace("\n", '') == "X",
                }
            

            # Objective Result
            objective_resultList= {
                "Interoperability Issue": html.find('th',string="Interoperability Issue").next_sibling.next_sibling.text.strip() if html.find('th',string="Interoperability Issue") is not None else '0',
                "Limited Success": html.find('th',string="Limited Success").next_sibling.next_sibling.text.strip() if html.find('th',string="Limited Success") is not None else '0',
                "Not Tested": html.find('th',string="Not Tested").next_sibling.next_sibling.text.strip() if html.find('th',string="Not Tested") is not None else '0', 
                "Success": html.find('th',string="Success").next_sibling.next_sibling.text.strip() if html.find('th',string="Success") is not None else '0',
            }
            #make maximum from all the values and get the key
            objectiveresult = max(objective_resultList, key=objective_resultList.get)

            # Summary
            summary = html.find(
                "div", string="Summary").find_next_sibling().text.strip("\n").replace("\n", '')

            # Recommendation
            recommentation = html.find(
                "div", string="Recommendations").find_next_sibling().text.strip("\n").replace("\n", '')
            
            div=html.find("div",string="Signed-up Capability Configurations")

            if(div is None):
                div=html.find("div",string="Planned Capability Configurations")
                
            trs=div.find_next_sibling().find("table").find_all("tr")[1:]

            capabilities=[x.find_all("td")[1].text.split()[0].strip() for x in trs]

            return OB(id, cycle, title, relevantstandards, scope, objectiveresult, summary, recommentation, description,[],capabilities)

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
        """Create a OB object from a JSON file"""

        try:

            with open(json_file, "r") as f:
                json_data = json.load(f)

            #check if key exists
            if "compability" not in json_data:
                json_data["compability"] = []
            
            json_data["compability"]=OB.sanitize(json_data["compability"])

            return OB(json_data["id"], json_data["cycle"], json_data["title"], json_data["relevantstandards"], json_data["scope"], json_data["objectiveresult"], json_data["summary"], json_data["recommendation"], json_data["description"], json_data["compability"],json_data["capabilitiesids"])
        except Exception as e:
            print(
                f"Exception occured while parsing {json_file}.", e, type(e))

            return None

    def export_to_json(self, path):
        """Export a OB object to a JSON file"""

        with open(f"{os.path.join(path, self.id)}.json", "w") as f:
            json.dump({
                "id": self.id,
                "cycle": self.cycle,
                "title": self.title,
                "relevantstandards": self.relevantstandards,
                "scope": self.scope,
                "objectiveresult": self.objectiveresult,
                "summary": self.summary,
                "recommendation": self.recommendation,
                "description": self.description,
                "compability": self.compability,
                "capabilitiesids":self.capabilitiesids
            }, f, indent=4)

    @staticmethod
    def sanitize(array):
        new_array=[]
        for x in array:
            if x==[]:
                continue
            new_array.append(x)
        return new_array

    def checkIfHaveCompabilityFromSameCycle(self, other):
        """Check if the OB object has a compability from the same cycle"""

        if not isinstance(other, OB):
            raise TypeError("Comparer must be of type OB")

        for compability in self.compability:
            if compability["year"] == other.cycle:
                return True
        return False

    def get_similarity(self, other):
        """Compute the similarity between two OB objects"""

        if not isinstance(other, OB):
            raise TypeError("Comparer must be of type OB")

        if self.checkIfHaveCompabilityFromSameCycle(other):
            return False

        focusArea=self.id.split("@")[1].strip()
        otherFocusArea=other.id.split("@")[1].strip()

        if focusArea != otherFocusArea:
            return False
        
        if not similarity(self.title, other.title,0.8):
            return False

        if not similarity(self.description, other.description,0.75):
            return False
        
        return True
    
    def exactSimilarity(self, other):
        """Choose the most similar OB object from a list of OB objects"""

        if not isinstance(other, OB):
            raise TypeError("Comparer must be of type OB")
        
        if self.checkIfHaveCompabilityFromSameCycle(other):
            return False

        focusArea=self.id.split("@")[1].strip()
        otherFocusArea=other.id.split("@")[1].strip()

        if focusArea != otherFocusArea:
            return False
        
        if self.title != other.title:
            return False
        
        if self.description != other.description:
            return False

        return True
    
    def addCompability(self, other,year):
        """Add compability to another OB object"""
            
        if not isinstance(other, OB):
                raise TypeError("Comparer must be of type OB")
    
        self.compability.append({"id":other.id,"year":year})
    