// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Tide.Normalize;
using Tide.Normalize.Capabilities;
using Tide.Normalize.Countries;
using Tide.Normalize.Fas;
using Tide.Normalize.Objectives;
using Tide.Normalize.Standards;
using Tide.Normalize.Tcs;
using Tide.Normalize.Tts;

Console.WriteLine("Note: existing entries from database will not be deleted!");
Console.WriteLine("Note: Press X to close!");
Console.WriteLine("1.Import nations. (Not db safe)");
Console.WriteLine("2.Import standards. (Not db safe)");
Console.WriteLine("3.Import focus areas. (Not db safe)");
Console.WriteLine("4.Import capabilities. (Not db safe)");
Console.WriteLine("5.Map capabilities to focus areas. (Not db safe)");
Console.WriteLine("6.Import objectives. (Not db safe)");
Console.WriteLine("7.Map objectives to focus areas. (Not db safe)");
Console.WriteLine("8.Map objectives to capabilities. (Not db safe)");
Console.WriteLine("9.Import test templates. (Not db safe)");
Console.WriteLine("10.Import test cases. (Not db safe)");
Console.WriteLine("11.Map objectives to test cases. (Not db safe)");
Console.WriteLine("12.Map objectives to test templates via test cases. (Not db safe)");

int option = 0;

while (true)
{
RELOAD:
    Console.WriteLine("****************************");
    Console.WriteLine("Enter option:");
    if (!int.TryParse(Console.ReadLine(), out option))
        option = 0;

    switch (option)
    {
        case 1:
            {
                try
                {
                    Console.WriteLine("Parsing nations, please wait ...");
                    CountryUtils.Save();
                    
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 2:
            {
                try
                {
                    Console.WriteLine("Parsing standards, please wait ...");
                    StandardsUtils.Save();

                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 3:
            {
                try
                {
                    Console.WriteLine("Parsing fas, please wait ...");
                    FasUtils.Save();

                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 4:
            {
                try
                {
                    Console.WriteLine("Parsing ccs, please wait ...");
                    CapabilitiesUtils.Save();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 5:
            {
                try
                {
                    Console.WriteLine("Mapping ccs and fas, please wait ...");
                    FasUtils.MapCapabilities();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 6:
            {
                try
                {
                    Console.WriteLine("Parsing objs, please wait ...");
                    ObjectiveUtils.Save();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 7:
            {
                try
                {
                    Console.WriteLine("Mapping obj and fas, please wait ...");
                    FasUtils.MapObjectives();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 8:
            {
                try
                {
                    Console.WriteLine("Mapping ccs and obj, please wait ...");
                    ObjectiveUtils.MapCapabilities();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 9:
            {
                try
                {
                    Console.WriteLine("Parsing tts, please wait ...");
                    TtUtils.Save();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 10:
            {
                try
                {
                    Console.WriteLine("Parsing tcs, please wait ...");
                    TcUtils.Save();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 11:
            {
                try
                {
                    Console.WriteLine("Mapping obj and tcs, please wait ...");
                    TcUtils.MapObjectives();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 12:
            {
                try
                {
                    Console.WriteLine("Mapping obj and tts, please wait ...");
                    TtUtils.MapObjectives();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 13:
            {
                try
                {
                    Console.WriteLine("Enhacing ...");
                    CapabilitiesUtils.ComputeSuccessRate();
                    CapabilitiesUtils.ComputeStandardSuccessRate();
                    CapabilitiesUtils.CalculateCapabilityPower();
                    CapabilitiesUtils.CalculateCapabilityInteroperability();

                    CapabilitiesUtils.CalculateCapabilityObjectiveInteroperability();
                    CapabilitiesUtils.CalculateObjectiveInteroperability();
                    CapabilitiesUtils.CalculateFaInteroperability();
                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 20:
            {
                try
                {
                    Console.WriteLine("Testing ...");
                    CapabilitiesUtils.CalculateCapabilityObjectiveInteroperability();
                    CapabilitiesUtils.CalculateObjectiveInteroperability();
                    

                    CapabilitiesUtils.CalculateFaInteroperability();

                }
                catch
                {
                    Console.WriteLine("Operation failed.");
                }
                goto RELOAD;
            }
        case 88:
        case 120:
            goto END;
        default: goto RELOAD;
    }
}

END:
return;