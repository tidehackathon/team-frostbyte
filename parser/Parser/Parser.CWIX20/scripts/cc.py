# I want to parse all html files as dom and get data from ../../../../../data/raw/cwix21/

from bs4 import BeautifulSoup
import json
import os

countGood = 0
countBad = 0
#define a class for this object
cc_example={
    "id":"CC-083",
    "cycle":"cwix22",
    "name":"ROU InCOP 4",
    "maturity": "",
    "compatibility": [],
    "operationaldomains": [ "" ],
    "tasks": [""],
    "warfarelevel": ["", ""],
    "organisationid":"cwix22:Cyber Command",
    "description": "InCOP is a C2 system that provides an integrated near-real time Common Operational Picture (COP). ",
    "standards":[
        "APP-6A",
        "APP-6B",
        "APP-6D",
        "MIL-STD-2525B",
        "MIL-STD-2525D"
    ],
    "interoperabilityachievements" : "",
    "interoperabilityimprovements" : "",
    "interoperabilitychallenges" : "",
    "country": "",
    "withdrawn": False
}

class CC:
    def __init__(self, id, cycle, name, maturity, operationaldomains, tasks, warfarelevel, organisationid, description, standards, interoperabilityachievements, interoperabilityimprovements, interoperabilitychallenges,country,withdrawn):
        self.id = id
        self.cycle = cycle
        self.name = name
        self.maturity = maturity
        self.operationaldomains = operationaldomains
        self.tasks = tasks
        self.warfarelevel = warfarelevel
        self.organisationid = organisationid
        self.description = description
        self.standards = standards
        self.interoperabilityachievements = interoperabilityachievements
        self.interoperabilityimprovements = interoperabilityimprovements
        self.interoperabilitychallenges = interoperabilitychallenges
        self.country = country
        self.compatibility = []
        self.withdrawn = withdrawn

    def __repr__(self):
        return f'CC({self.id}, {self.cycle}, {self.name}, {self.maturity}, {self.operationaldomains}, {self.tasks}, {self.warfarelevel}, {self.organisationid}, {self.description}, {self.standards}, {self.interoperabilityachievements}, {self.interoperabilityimprovements}, {self.interoperabilitychallenges})'

    def __str__(self):
        return f'CC({self.id}, {self.cycle}, {self.name}, {self.maturity}, {self.operationaldomains}, {self.tasks}, {self.warfarelevel}, {self.organisationid}, {self.description}, {self.standards}, {self.interoperabilityachievements}, {self.interoperabilityimprovements}, {self.interoperabilitychallenges})'

    def __eq__(self, other):
        return self.id == other.id

    def __hash__(self):
        return hash(self.id)

    def to_dict(self):
        return {
            "id": self.id,
            "cycle": self.cycle,
            "name": self.name,
            "maturity": self.maturity,
            "operationaldomains": self.operationaldomains,
            "tasks": self.tasks,
            "warfarelevel": self.warfarelevel,
            "organisationid": self.organisationid,
            "description": self.description,
            "standards": self.standards,
            "interoperabilityachievements": self.interoperabilityachievements,
            "interoperabilityimprovements": self.interoperabilityimprovements,
            "interoperabilitychallenges": self.interoperabilitychallenges,
            "country": self.country,
            "compatibility": self.compatibility,
            "withdrawn": self.withdrawn
        }

    def to_json(self):
        return json.dumps(self.to_dict())
    
    def to_json_file(self, file_path):
        with open(file_path, 'w') as f:
            json.dump(self.to_dict(), f,indent=4)

# Define a function to extract data from a single HTML file
def extract_data_from_file(file_path):
    global countGood
    global countBad
    with open(file_path, 'r') as f:
        html = f.read()
    soup = BeautifulSoup(html, 'html.parser')
    # get the body of the html
    body = soup.find('body')
    # get the div with id = "content"
    try:
        content = body.find('div', id='content')
        # get the div with id = "bodyContent"
        bodyContent = content.find('div', id='bodyContent')

        cycle = bodyContent.contents[1]
        cycle = cycle.text.strip().split(' ')[1]
        
        maturityTD=bodyContent.findAll('table', class_='wikitable')[2].findAll('td')[1]
       
        banner=bodyContent.find('div',class_='banner-center').text
        banner=banner.split(' ')[1:]
        banner=[x.strip() for x in banner]
        banner=' '.join(banner)

        
        bannerBottom=bodyContent.find('div',class_='banner-bottom-center')
        withDrawnText=bannerBottom.text.strip()
        withdrawn=False
        if(withDrawnText=='Withdrawn'):
            withdrawn=True
     

        bannerRight=bodyContent.find('div',class_='banner-right')
        country=bannerRight.find('div').find('a')['title']

        name = banner.strip()
        maturity = maturityTD.text.strip()

        
        description=bodyContent.find('span',id='Description').parent.next_sibling.next_sibling.text.strip()

        tag=bodyContent.find('div',id='Tasks').find('div',class_='gallery')

        tasks=[]
        try:
            tasks=[x['title'] for x in tag.findAll('a')]
        except:
            pass

        interoperabilityachievements=bodyContent.find('div',id='Final_Report').find('div',string='Interoperability Achievements').next_sibling.text

        interoperabilityachievements=[ x.strip() for x in interoperabilityachievements.split('\n')] 
        interoperabilityachievements=[x for x in interoperabilityachievements if x][1:]

        interoperabilityimprovements=[]
        try:

            interoperabilityimprovements=bodyContent.find('div',id='Final_Report').find('div',string='Interoperability Improvements').next_sibling.text

            interoperabilityimprovements=[ x.strip() for x in interoperabilityimprovements.split('\n')]
            interoperabilityimprovements=[x for x in interoperabilityimprovements if x][1:]
        except:
            pass

 

        interoperabilitychallenges=bodyContent.find('div',id='Final_Report').find('div',string='Interoperability Challenges').next_sibling.text

        interoperabilitychallenges=[ x.strip() for x in interoperabilitychallenges.split('\n')]
        interoperabilitychallenges=[x for x in interoperabilitychallenges if x][1:]


        standardsListTag=bodyContent.find('span',id='Relevant_Standards').parent.next_sibling.next_sibling

        standards=[]
        if(standardsListTag.find('span')):
            standards=[x['title'] for x in standardsListTag.find('span').findAll('a')]

           #relative path of filepath
        fileName=file_path.split('/')[-1].replace('.html','.json')
        #take last12 characters of file path
        fileId=fileName[-12:]
        cycle=cycle.replace('CWIX','20')
        # Extract data from the soup object and store it in a dictionary or list
        ccObj = CC(
            id = fileId[1:-5],
            cycle = cycle,
            name = name,
            maturity = maturity,
            operationaldomains = [],
            tasks = tasks,
            warfarelevel = [],
            organisationid = "",
            description = description,
            standards = standards,
            interoperabilityachievements = interoperabilityachievements,
            interoperabilityimprovements = interoperabilityimprovements,
            interoperabilitychallenges = interoperabilitychallenges,
            country=country,
            withdrawn=withdrawn,
        )
     
        pathToSave='../../../../../data/normalized/cwix20/cc/'+fileId
        ccObj.to_json_file(pathToSave)
        countGood+=1
    except:
        countBad+=1
        pass

# Define a function to loop through all HTML files in a directory and extract data from each file
def extract_data_from_directory(directory_path):
    files=[x for x in os.listdir(directory_path)]
    for file_name in files:
        file_path = os.path.join(directory_path, file_name)
        extract_data_from_file(file_path)
        
    
    print(countGood)
    print(countBad)
    rate=countGood/(countGood+countBad)
    print(rate)  

# Extract data from all HTML files in a directory and save it to a JSON file
extract_data_from_directory('../../../../../data/raw/cwix20/cc')

