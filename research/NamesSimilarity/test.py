import os
import Leivenstien as lv
import pandas as pd
import json
import sys

from collections import Counter

def are_arrays_similar(arr1, arr2):
    # Determine which array is smaller
    smaller_arr = arr1 if len(arr1) < len(arr2) else arr2
    larger_arr = arr1 if len(arr1) >= len(arr2) else arr2

    # Determine the threshold for a match
    threshold = 0.5 * len(smaller_arr)

    # Count the number of matches between the two arrays
    num_matches = sum(1 for element in smaller_arr if element in larger_arr)

    # Determine if the number of matches exceeds the threshold
    return num_matches >= threshold



def get_files(path):
    files=[x for x in os.listdir(path)]
    files_path=[]
    for file_name in files:
        if(file_name[0]=='.'):
            continue
        file_path = os.path.join(path, file_name)
        files_path.append(file_path)
    return files_path


def extract_name_from_file(file_path):
    #file is a json that contains the name
    #read the file
    with open(file_path, 'r') as f:
        data=json.load(f)
    #get the name
    name = data['name']
    return name

def extract_data_from_file(file_path):
    #file is a json that contains the name
    #read the file
    with open(file_path, 'r') as f:
        data=json.load(f)
    #get the name
    name = data['name']
    id = data['id']
    country = data['country']
    standards = data['standards']
    withdrawn = data['withdrawn']
    return name, id, country, standards, withdrawn

def create_excel(names,year):
    df = pd.DataFrame(names, columns = ['name'])
    df.to_excel(r'names'+year+'.xlsx', index = False)

def excel(year):
    yearCode = year[2:]
    #get the files
    files_path = get_files('../../../data/normalized/cwix'+yearCode+'/cc')
    #extract the names
    names=[]
    for file_path in files_path:
        name = extract_name_from_file(file_path)
        names.append(name)
    #create the excel
    create_excel(names,year)

def read_excel(year):
    yearCode = year[2:]
    df = pd.read_excel('names'+year+'.xlsx')
    names = df['name'].tolist()
    return names

def compare_names(year1,year2,algorithm):
    names1 = read_excel(year1)
    names2 = read_excel(year2)
    similar = []
    for name1 in names1:
        for name2 in names2:
            if algorithm(name1,name2):
                similar.append([name1,name2])
    return similar

def compareExactNames(year1,year2):
    names1 = read_excel(year1)
    names2 = read_excel(year2)
    similar = []
    for name1 in names1:
        for name2 in names2:
            if name1 == name2:
                similar.append([name1,name2])
    return similar

def checkIfNamesAreAlreadyInList(name1,name2,similar):
    for pair in similar:
        if name1 == pair[0] or name2 == pair[1]:
            return True
    return False

def compareStandards(standards1,standards2):
    if are_arrays_similar(standards1,standards2):
        return True
    else:
        return False

def compareComplete(name1,name2,algorithmForName,country1,country2,standards1,standards2):
    if country1 != country2:
        return False
    if not compareStandards(standards1,standards2):
        return False
    if algorithmForName(name1,name2):
        return True
    else:
        return False
    
def compareCompleteNames(year1,year2,algorithmForName):
    yearCode1 = year1[2:]
    yearCode2 = year2[2:]
    #get the files
    files_path1 = get_files('../../../data/normalized/cwix'+yearCode1+'/cc')
    files_path2 = get_files('../../../data/normalized/cwix'+yearCode2+'/cc')
    #extract the names
    similar = []
    for file_path1 in files_path1:
        name1, id1, country1, standards1,_ = extract_data_from_file(file_path1)
        for file_path2 in files_path2:
            name2, id2, country2, standards2,_ = extract_data_from_file(file_path2)
            try:
                if compareComplete(name1,name2,algorithmForName,country1,country2,standards1,standards2):
                    similar.append([name1,name2])
            except:
                continue
    return similar
    

def createExcelSimilar(similar,year1,year2,algorithm):
    df = pd.DataFrame(similar, columns = ['name'+year1,'name'+year2])
    df.to_excel(r'similar'+year1+year2+algorithm+'.xlsx', index = False)

def sanitize(files_path):
    for file_path in files_path:
        with open(file_path, 'r') as f:
            data=json.load(f)
        compatibility = data['compatibility']
        compatibility.remove([])
        data['compatibility'] = compatibility
        with open(file_path, 'w') as f:
            json.dump(data, f)

def updateJsonWithCompatibility(year1,year2,similarArray):
    yearCode1 = year1[2:]
    yearCode2 = year2[2:]
    #get the files
    files_path1 = get_files('../../../data/normalized/cwix'+yearCode1+'/cc')
    files_path2 = get_files('../../../data/normalized/cwix'+yearCode2+'/cc')

    print('Similar: '+str(len(similarArray)))

    # #extract the names
    for file_path1 in files_path1:
        name1, id1, country1, standards1,withdrawn1 = extract_data_from_file(file_path1)
        for file_path2 in files_path2:
            name2, id2, country2, standards2,withdrawn2 = extract_data_from_file(file_path2)
            if withdrawn1 or withdrawn2:
                continue
            if [name1,name2] in similarArray:
                #update the json
                with open(file_path1, 'r') as f:
                    data=json.load(f)
                data['compatibility'].append({"id":id2,"year":year2})
                with open(file_path1, 'w') as f:
                    json.dump(data, f,indent=4)
                with open(file_path2, 'r') as f:
                    data=json.load(f)
                data['compatibility'].append({"id":id1,"year":year1})
                with open(file_path2, 'w') as f:
                    json.dump(data, f,indent=4)

if __name__ == "__main__":
    excel(sys.argv[1])
    excel(sys.argv[2])
    year1=sys.argv[1]
    year2=sys.argv[2]
    exactSimilar = compareExactNames(year1,year2)
    print('Exact: '+str(len(exactSimilar)))
    similar = compareCompleteNames(year1,year2,lv.leivSimilarityBy5)
    print('Similar: '+str(len(similar)))
    goodOnes = exactSimilar
    for pair in similar:
        if not checkIfNamesAreAlreadyInList(pair[0],pair[1],exactSimilar):
            goodOnes.append(pair)
    
    updateJsonWithCompatibility(year1,year2,goodOnes)

