import os
from models.TT import TT
from models.OB import OB
from pprint import pprint
import csv


def parser(path, cycles, items):
    for cycle in cycles:
        for item in items:
            total, success, failed = 0, 0, 0
            for filename in os.listdir(path.format("raw", cycle, item)):
                if filename.endswith(".html"):
                    absolute_path = os.path.abspath(os.path.join(
                        path.format("raw", cycle, item), filename))
                    if item == "tt":
                        tt = TT.from_html_file(
                            absolute_path, cycle=int(f"20{cycle}"))
                        if tt is not None:
                            tt.export_to_json(path.format(
                                "normalized", cycle, item))
                            success += 1
                        else:
                            failed += 1
                    else:
                        if(cycle == "19"):
                            ob=OB.from_html_file2019(absolute_path,cycle=int(f"20{cycle}"))
                        else:
                            ob = OB.from_html_file(
                                absolute_path, cycle=int(f"20{cycle}"))
                        if ob is not None:
                            ob.export_to_json(path.format(
                                    "normalized", cycle, item))
                            success += 1
                        else:
                            failed += 1

                    total += 1

            print(
                f"[{int(f'20{cycle}')}][{item.upper()}] - Total: {total}, Success: {success}, Failed: {failed}.")


if __name__ == "__main__":


    path = "/Users/teodorcazamir/Desktop/frostbyte/data/{0}/cwix{1}/{2}"
    cycles = ["19", "20", "21", "22"]
    items = ["tt"]

    parser(path, cycles, items)

    tt_19 = []
    for filename in os.listdir(path.format("normalized", "20", "tt")):
        if filename.endswith(".json"):
            absolute_path = os.path.abspath(os.path.join(
                path.format("normalized", "20", "tt"), filename))
            tt_19.append(TT.from_json_file(absolute_path))

    for tt in tt_19:
        tt.compute_diffusion(tt_19)

    tt_20 = []
    for filename in os.listdir(path.format("normalized", "20", "tt")):
        if filename.endswith(".json"):
            absolute_path = os.path.abspath(os.path.join(
                path.format("normalized", "20", "tt"), filename))
            tt_20.append(TT.from_json_file(absolute_path))

    for tt in tt_20:
        tt.compute_diffusion(tt_20)

    tt_21 = []
    for filename in os.listdir(path.format("normalized", "21", "tt")):
        if filename.endswith(".json"):
            absolute_path = os.path.abspath(os.path.join(
                path.format("normalized", "21", "tt"), filename))
            tt_21.append(TT.from_json_file(absolute_path))

    for tt in tt_21:
        tt.compute_diffusion(tt_21)

    tt_22 = []
    for filename in os.listdir(path.format("normalized", "22", "tt")):
        if filename.endswith(".json"):
            absolute_path = os.path.abspath(os.path.join(
                path.format("normalized", "22", "tt"), filename))
            tt_22.append(TT.from_json_file(absolute_path))

    for tt in tt_22:
        tt.compute_diffusion(tt_22)

    # Diffusion
    '''
    mappings = {}
    for tt in tt_21:
        tt.compute_diffusion(tt_21)

        tt_22 = []
    # for filename in os.listdir(path.format("normalized", "22", "tt")):
    #     if filename.endswith(".json"):
    #         absolute_path = os.path.abspath(os.path.join(
    #             path.format("normalized", "22", "tt"), filename))
    #         tt_22.append(TT.from_json_file(absolute_path))

    for tt in tt_22:
        tt.compute_diffusion(tt_22)

    # # Diffusion
    # '''

    # Timeline
    
    for tt in tt_19:
        tt.compute_timeline([tt_20, tt_21, tt_22])

    for tt in tt_20:
        tt.compute_timeline([tt_19, tt_21, tt_22])

    for tt in tt_21:
        tt.compute_timeline([tt_19, tt_20, tt_22])

    for tt in tt_22:
      tt.compute_timeline([tt_19, tt_20, tt_21])

    # '''
    # with open("timeline.csv", "w") as f:
    #     writer = csv.writer(f)
    #     for tt in tt_21:
    #         if tt.timeline is not None:

    #             # Write cycle numbers
    #             to_write = [tt.cycle, '']
    #             for cycle, obj in tt.timeline.items():
    #                 to_write.append(cycle)
    #             writer.writerow(to_write)

    #             # Write ids
    #             to_write = [tt.id, '']
    #             for cycle, obj in tt.timeline.items():
    #                 to_write.append(obj['id'])
    #             writer.writerow(to_write)

    #             # Write purposes
    #             to_write = [tt.purpose, '']
    #             for cycle, obj in tt.timeline.items():
    #                 tt2_object = [tt2 for tt2 in tt_22 if tt2.id == obj['id']][0]
    #                 to_write.append(tt2_object.purpose)
    #             writer.writerow(to_write)

    #             # Write steps
    #             to_write = [list(map(lambda step: step['description'], tt.steps)), '']
    #             for cycle, obj in tt.timeline.items():
    #                 tt2_object = [tt2 for tt2 in tt_22 if tt2.id == obj['id']][0]
    #                 to_write.append(
    #                     list(map(lambda step: step['description'], tt2_object.steps)))
    #             writer.writerow(to_write)

    #             # Write similarities
    #             to_write = ['Similarity', '']
    #             for cycle, obj in tt.timeline.items():
    #                 to_write.append(obj['similarity'])
    #             writer.writerow(to_write)

    #             writer.writerow(['', ''])
    #             writer.writerow(['', ''])
    # '''
