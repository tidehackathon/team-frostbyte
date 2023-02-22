import editdistance

def leivSimilarity(name1, name2):
    # Split the application names into individual words
    words1 = name1.split()
    words2 = name2.split()

    # Compute the Levenshtein distance between the last word of the names and the version number
    dist = editdistance.eval(words1[-1], words2[-1])

    if len(words1) == len(words2):
        # The names have the same number of words, so they must match exactly except for the version number
        return words1[:-1] == words2[:-1] and dist <= 2
    elif len(words1) == len(words2) + 1:
        # name1 has an extra word (presumably a version number)
        return words1[:-1] == words2 and dist <= 2
    elif len(words2) == len(words1) + 1:
        # name2 has an extra word (presumably a version number)
        return words2[:-1] == words1 and dist <= 2
    else:
        # The names have a different number of words and are not considered a match
        return False
    
def leivSimilarityBy5(name1,name2):
    words1 = name1.split()
    words2 = name2.split()

    # Compute the Levenshtein distance between the last word of the names and the version number
    dist = editdistance.eval(words1[-1], words2[-1])

    if len(words1) == len(words2):
        # The names have the same number of words, so they must match exactly except for the version number
        return words1[:-1] == words2[:-1] and dist <= 5
    elif len(words1) == len(words2) + 1:
        # name1 has an extra word (presumably a version number)
        return words1[:-1] == words2 and dist <= 5
    elif len(words2) == len(words1) + 1:
        # name2 has an extra word (presumably a version number)
        return words2[:-1] == words1 and dist <= 5
    else:
        # The names have a different number of words and are not considered a match
        return False

def leivSimilarity0(name1,name2):
    words1 = name1.split()
    words2 = name2.split()

    # Compute the Levenshtein distance between the last word of the names and the version number
    dist = editdistance.eval(words1[0], words2[0])

    if len(words1) == len(words2):
        # The names have the same number of words, so they must match exactly except for the version number
        return words1[0] == words2[0] and dist <= 5
    elif len(words1) == len(words2) + 1:
        # name1 has an extra word (presumably a version number)
        return words1[:-1] == words2 and dist <= 5
    elif len(words2) == len(words1) + 1:
        # name2 has an extra word (presumably a version number)
        return words2[:-1] == words1 and dist <= 5
    else:
        # The names have a different number of words and are not considered a match
        return False
