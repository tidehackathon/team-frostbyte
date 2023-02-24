<p align="center">
  <img width="400" height="400" src="frostbyte.svg">
</p>

Solution is organised in 5 directories which contains scripts and applications that operates individually.

|Component name|Description|
|-------------------------------|-----------------------------|
|`Download`| Contains the application which can be used to do the scrapping (download) process from Tide |
|`Parser`| Contains the application and scripts which may be run in order to obtain a first form of data (stored as json files) and the scripts used to do a part of enhancement |
|`Research`| Thid directory holds the scripts / applications used for POC or testing different ideas which may be used in other solution applications |
|`Scripts`| Holds python scripts which are used to do most of enhancement and data augmentation  |
|`Tide`| This directory represents the host for web server app and console applications which store the data created by all other components |

# 1. Code
## 1.1.Scrapping
To acquire information from the TIDE platform, the solution in the Downloader directory must be utilized. It is imperative that the resulting data adheres to the appropriate folder structure for each year as established within the repository, with the option to generate these folders automatically set as default.

Pages needed to be downloaded:
1. Test Template
2. Test Case
3. Capability
4. Objectives
5. Focus Areas
6. Nations

#### Important !!!!!
This component do multiple requests in parallel, so be careful to not make a DDOS to tide. 
#### Configuration 
* To change request time, modify the constants MAX_DEGREE_OF_PARALELLISM and DELAY in Utils.cs file (Downloader.Commons csproj).
* In each program.cs file modify the value of username and password used to download data. 
* Uncomment type of object (TT, TC ...) which must be downloaded (Utils.Download method)

#### Requirments
* NET 6 SDK & Runtime;
* IDE;

## 1.2. Parsing
The objective of this application is to extract relevant data from HTML pages and subsequently generate JSON files that conform to the corresponding JSON schemas for each type of entity, namely TT, OB, CC, among others. In addition, the application aims to correlate entities by performing a search for the same identity of a particular entity across multiple years.

As part of its functionality, the application also includes a feature to rectify formatting errors that may arise due to incorrect standards provided by users on CCs pages. This ensures that the resulting JSON files are accurate and comply with the appropriate formatting guidelines.

* The parsing process is split in 2 sets of scripts, developed in 2 different technologies. It is organized as a .NET project, but for each year, there are .NET components and in the scripts folder, the python parsers can be found.

#### Important steps
* extract information from HTML pages and converting it into JSON. 
* establish a correlation between entities, by searching across multiple years for instances of the same identity associated with a particular entity;
* Fix formatting errors, like incorrect standards written by users in CCs pages;

#### Configuration
* There is no need for any additional configuration.

#### Run
* compile & run the .net project for desired cwix cycle;
* at present, the Python scripts do not have an auto-run feature. Therefore, to execute the code, methods must be manually invoked from the main function.

#### Requirments
* NET 6 SDK & Runtime;
* IDE;
* Python

## 1.3. Normalize
The normalization utility serves the purpose of converting and parsing JSON normalized data, establishing correlations with other relevant entities, rectifying errors, and precomputing relevant indicators and KPIs.
The utility provides a command-line interface that allows users to choose from a selection of available steps. When opend the fallowing prompt will be presented to user:



> Note: existing entries from database will not be deleted!
Note: Press X to close!
1.Import nations. (Not db safe) 
2.Import standards. (Not db safe)
3.Import focus areas. (Not db safe)
4.Import capabilities. (Not db safe)
5.Map capabilities to focus areas. (Not db safe)
6.Import objectives. (Not db safe)
7.Map objectives to focus areas. (Not db safe)
8.Map objectives to capabilities. (Not db safe)
9.Import test templates. (Not db safe)
10.Import test cases. (Not db safe)
11.Map objectives to test cases. (Not db safe)
12.Map objectives to test templates via test cases. (Not db safe)

#### Configuration
* In the Tide.Normalizer project, the file appsettings.josn is available for configuration;
* Change the path to point to the location where data is stored;
* Change the array with cwix years. Note that years must be written in the yy format;
* Change the connection string so it points to a MSSQL server where the database is deployed.

#### Run
* compile & run the .net project;
* use the GUI to enter options;

#### Requirments
* NET 6 SDK & Runtime;
* IDE;

## 1.4. Web 
This utility is designed to provide users with a user-friendly web-based platform for accessing data from the data store. Its primary objective is to enable the plotting of data in a visually engaging and easy-to-interpret manner, thereby facilitating enhanced understanding and decision-making.

#### Configuration
* In the Tide.Dashboard project, the file appsettings.josn is available for configuration;
* Change the path to point to the location where data is stored;
* Change the array with cwix years. Note that years must be written in the yy format;
* Change the connection string so it points to a MSSQL server where the database is deployed.

###$ Run
* compile & run the .net project;

## 1.5 Database
To create the database, the instructions outlined in script.sql must be executed. It is essential that the stored procedures are created, as they play a critical role in the enhancement process.

## 1.6 Ndpp
Not ready to be commited yet.

# 2. Usage example
* Utilize the data provided in section 1.1 to acquire a cycle's worth of data from the TIDE portal. Expected result : html files;
* Normalize all entities (CCs, TTs, TCs, etc.) using the parser, taking care to employ both C# (for TCs and FAs) and Python scripts (for the rest). Expected result : .json files for all entities;
* Establish correlations between entity identities across multiple cwix cycles using the correlation functions available in the Python scripts.
* Create database;
* Execute Tide.Normalizer and proceed with the steps numbered 1 through 10 to ensure the import of all relevant data. It is essential to ensure that the script is configured correctly, as outlined earlier.
* Execute the fallowing stored procedures from sql, in this order : enh_tt_maturity, enh_tt_tc_count, enh_tt_anomalies, chart_all_tt_year_anomalies;
* Execute the steps numbered 11 through 13 to establish data correlations.
* Execute the fallowing stored procedures from sql : enh_obj_tc
* Run web project and explore the pages. For the moment, menu is not published:
  * capability page : /capability?capabilityId= X  -> where X is any id from dbo.Capabilities table;
  * nation page : /nation?nationId= X -> where X is any id from dbo.Nations table;
  * multidomain page : /multidomain
  * anomalies page : /anomalies

# 3. Dialy report

### Day 1
* After analyzing the data in Azure Storage, we have concluded that there is insufficient information available for us to extract the relevant data necessary to calculate the desired KPIs. Therefore, we have decided to scrape the data from the CWIX portal for the years 2019-2022 (TTs, TCs, CCs, FAs).
* The next step involved extracting data from the HTML and normalizing it. This operation presented a challenge in itself, as the entities in the CWIX portal do not maintain their form over the years.
* Once normalized, we began the process of temporally correlating the capabilities and test templates, which presented several issues as described in the "Challenges" section.

### Day 2
* We have started the data mining process to obtain a format that allows for easy extraction of the necessary information for calculating the KPIs.
* After running the data extraction processes, we noticed a very low level of interoperability. Upon exploring the data, we identified several issues related to temporal correlation of the information, as described in the "Challenges" section, which we have referred to as "anomalies."
* We have defined and executed the process for correcting the anomalies, including the diffusion calculation process mentioned in the "Challenges" section.
* We have completed the process of extracting the necessary indicators for generating visual charts.

### Day 3
* After analyzing the data and indicators available, which provided us with a multitude of options, we had to choose a limited set of indicators and display methods to present the most relevant information to the user in a way that is easily understandable and quickly comprehensible.
* The first set of charts was designed to show the average deviation of the entities, as well as how it has evolved over the CWIX cycles.
* The next major step was to display the information about the capability regarding the evolution of interoperability over time, the adoption of new standards, and the involvement in multiple domains throughout the years, as well as interoperability per standard.
* After successfully extracting all the relevant capability indicators and correlating them in a representative manner for our purposes, we were able to generate graphs for each nation to show its involvement in multiple domains and the level of interoperability over the CWIX cycles.

### Day 4
* We have defined the most important indicators and their visualization methods in relation to the macro level of the CWIX cycles.
* We have created a display method that allows for the visualization of the evolution of interoperability in focus areas over time.
* We have combined all the available indicators to generate a more abstract and simplistic form of visualization of the essence of a CWIX exercise, which allows for easy visualization of the multi-domain interaction and the degree of interoperability between them, as well as the success/failure rate in relation to the tests between them.
* We have started to explore the new data on the "NATO Defense Planning Process" that has been made available.
* After a few attempts, we realized that using different correlation methods, we can extract the necessary standards for each NDPP capability, which allows us to correlate them with the capabilities in CWIX.
* We have defined the method for correlating the NDPP capabilities with the extracted indicators, as well as the new indicators that arise from the new correlation.
* For each capability, we have displayed its correlation with the NDPP capabilities. This has allowed us to estimate which NDPP capabilities a country is providing, as requested by NATO.
* We have defined an easy-to-use method for visualizing the CWIX support in relation to NDPP, as well as the evolution of this connection over time.

# 4. Challenges
#### 4.1 Temporal correlation
In order to extract important indicators such as maturity and to better track the entities over time, temporal correlation between entities throughout the cycles is critical.
##### 4.1.1 Capabilities
Temporal correlation of capabilities is complicated for 2 main reasons:
* The CWIX portal has no restrictions on the capability name, which is the only relevant indicator for capability and is a free text field. Therefore, capability leaders introduce different names for the capability over time, making temporal correlation challenging.
* Capabilities are completely different between FMN spirals, to the point where application versions are not compatible with each other, and a version for a higher FMN spiral does not support a lower spiral version. Therefore, even if the only difference in the application name is a single letter (SP3 -> SP4), we are talking about two different applications. This further complicates temporal correlation of capabilities.

**Solution :** 
* We have used multiple mathematical algorithms for similarity comparison (Jaccard, Levenshtein, fuzzy matching), applied linearly to ensure that the limitations of one are compensated by another formula, both for the capability name and the standards. We made sure that there is at least a consistent percentage of standards between versions.
* At the same time, we have used Azure Cognitive Services to extract relevant tokens, such as FMN spiral, so that we can differentiate between applications.

##### 4.1.2 Test Templates
The main problem with Test Templates is how they are freely created by users, as this leads to duplicating templates both between focus areas and surprisingly, within the same focus area.
Therefore, the templates raise the following issues:
* Duplicating templates between focus areas reduces the ability to track interactions between domains.
* Duplicates end up being used mistakenly by those who create tests, which reduces the actual correlation level of the real template with the test cases.
* Temporal correlation will be done probabilistically, as there are duplicates between years that are 100% identical, making it impossible to track the evolution of the template correctly over time.

**Solution:**
* We have calculated a diffusion degree for the templates from a year using the techniques mentioned above. We analyzed both the purpose of the templates as well as the necessary steps for performing the template, both in content and form and order, in order to detect template duplicates.
* We have temporally correlated between CWIX cycles only the main instances of the templates.

#### 4.2 Anomalies
After analyzing the collected data, three major problems were detected that were generating anomalies:
* Duplicate test templates -> problem corrected through diffusion.
* Incorrect test cases (e.g. one partner said they have interoperability issues, the other partner said they did not test) -> problem corrected by ignoring the tests and updating the actual number of tests.
* Tests planned in the CWIX portal but performed in IOCORE -> problem corrected by ignoring the tests and templates and updating the actual number of tests.

# 5. Indicators
##### 5.1 Maturity
* The maturity of the Test Templates can be calculated as a logarithmic deviation over time, taking into account the persistence of the template over time and the acceptance rate per cycle.
* The maturity of a capability can be calculated as a logarithmic deviation over time in relation to the success rate and error rate of tests correlated with the maturity declared by the Capability Lead, as well as the anomalies detected in tests.

##### 5.2 Test Case Weight
The weight of a test case can be calculated as a progression between the dates of the values defined for the result types correlated with the maturity of the template from which it derives (if it is a duplicate, the parent is used), as well as the maturity of the partner with whom it is testing.

##### 5.3 Capability - Standard Interoperability
For each standard that the capability has declared and created and performed tests for, a relevant interoperability score only for the capability can be calculated as an average of the test's weight in relation to the success rate and the deduction given by the failure rate, and the normalization of anomalies.
##### 5.4 Capability Interoperability
For each capability, interoperability can be calculated as a weighted average of the interoperability value per standard, along with the standard weight.
For each capability, 2 degrees of interoperability have been calculated:
* **Base Interoperability** represents the degree of interoperability given by tests on the capability's traditional standards. If in the following year the capability no longer tests on the standards from the current cycle, the base interoperability does not decrease. At the same time, the base interoperability does not decrease with a high failure rate on the newly adopted standards.
* **Current Interoperability** represents the degree of interoperability given by the tests on the newly adopted standards.
