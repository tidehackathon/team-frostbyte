import os
from models.OB import OB
import pandas as pd
import sys 

def get_files(path):
    files=[x for x in os.listdir(path)]
    files_path=[]
    for file_name in files:
        if(file_name[0]=='.'):
            continue
        file_path = os.path.join(path, file_name)
        files_path.append(file_path)
    return files_path

def getObFromFiles(files_path):
    #extract the names
    ob=[]
    for file_path in files_path:
        ob.append(OB.from_json_file(file_path))
    return ob

def createCompability(year1,year2):
    year1Code = year1[2:]
    year2Code = year2[2:]
    #get the files
    files_path1 = get_files('../../data/normalized/cwix'+year1Code+'/ob')
    files_path2 = get_files('../../data/normalized/cwix'+year2Code+'/ob')
    year1Ob = getObFromFiles(files_path1)
    year2Ob = getObFromFiles(files_path2)

    similarOb = []

    for ob1 in year1Ob:
        for ob2 in year2Ob:
            if ob1.exactSimilarity(ob2):
                similarOb.append([ob1,ob2])
                ob1.addCompability(ob2,year2)
                ob2.addCompability(ob1,year1)
                continue
            if ob1.get_similarity(ob2):
                similarOb.append([ob1,ob2])
                ob1.addCompability(ob2,year2)
                ob2.addCompability(ob1,year1)
    
    print('Similar OBs: ',len(similarOb))

    for ob1 in year1Ob:
        ob1.export_to_json('../../data/normalized/cwix'+year1Code+'/ob')
    for ob2 in year2Ob:
        ob2.export_to_json('../../data/normalized/cwix'+year2Code+'/ob')


def forVisualization(ob1,ob2,similarOb,year1,year2):
    ob1FromSimilar = [x[0] for x in similarOb]
    ob2FromSimilar= [x[1] for x in similarOb]
    
    createExcel(ob1FromSimilar,ob2FromSimilar,year1,year2)
    
def createExcel(ob1List,ob2List,year1,year2):
    title1 = [x.title for x in ob1List]
    title2 = [x.title for x in ob2List]
    description1 = [x.description for x in ob1List]
    description2 = [x.description for x in ob2List]

    column1=[]
    column2=[]

    for i in range(len(title1)):
        column1.append(title1[i])
        column1.append(description1[i])
        column1.append('')

    for i in range(len(title2)):
        column2.append(title2[i])
        column2.append(description2[i])
        column2.append('')

    df= pd.DataFrame(columns=[year1,year2])
    df[year1]=column1
    df[year2]=column2

    df.to_excel(r'compability'+year1+'-'+year2+'.xlsx', index = False)

    
if __name__ == "__main__":
    year1=sys.argv[1]
    year2=sys.argv[2]
    createCompability(year1=year1,year2=year2)