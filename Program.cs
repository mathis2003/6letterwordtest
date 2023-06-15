using System.Text.RegularExpressions;

public class WordCombos
{
    private const int MAX_WORD_LENGTH = 6;
    public static void Main(string[] args)
    {
        if (args.Length == 0) throw new ArgumentException("give a correct filename as an argument");
        string fileName = args[0];

        // sort of a lookup table, used to quickly find 
        // morphemes by the combination of their length and first character
        MorphemeFinder morphemeFinder = new MorphemeFinder(MAX_WORD_LENGTH);

        List<string> resultWords = new List<string>();
 
        using (StreamReader reader = new StreamReader(fileName))
        {
            string line;
            while ((line = reader.ReadLine()) != null) {
                line = line.Trim();
                if (line.Length < MAX_WORD_LENGTH) morphemeFinder.addMorpheme(line);
                else if (line.Length == MAX_WORD_LENGTH) resultWords.Add(line);
            }
        }

        ComboFinder comboFinder = new ComboFinder(morphemeFinder);

        foreach (string resultWord in resultWords) {
            foreach (List<string> combo in comboFinder.findCombos(resultWord)) {
                Console.WriteLine(string.Join("+", combo) + "=" + resultWord);
            }
        }
    }
}

// For lack of a better term, I'll call the words that are to be combined "morphemes".
// This originates from linguistics and is described as 
// "the smallest meaningful constituent of a linguistic expression".
// See: https://en.wikipedia.org/wiki/Morpheme
public class MorphemeFinder
{
    private List<string>[,] hashmaps;

    private int alphabetSize = 26;
    public MorphemeFinder(int maxWordLength) {
        hashmaps = new List<string>[maxWordLength, alphabetSize];
        for (int i = 0; i < maxWordLength; i++) {
            for (int j = 0; j < alphabetSize; j++) {
                hashmaps[i, j] = new List<string>();
            }
        }
    }

    public void addMorpheme(string morpheme) {
        List<string> morphemeList = hashmaps[morpheme.Length, morpheme[0] - 'a'];
        if (!morphemeList.Contains(morpheme)) morphemeList.Add(morpheme);
    }

    public List<string> findMorphemes(int morphemeLength, char firstCharacter) {
        return hashmaps[morphemeLength, firstCharacter - 'a'];
    }
}

public class ComboFinder {

    private MorphemeFinder morphemeFinder;
    public ComboFinder(MorphemeFinder morphemeFinder) {
        this.morphemeFinder = morphemeFinder;
    }
    public List<List<string>> findCombos(string resultWord) {
        return findCombosRec(resultWord, 0);
    }

    public List<List<string>> findCombosRec(string resultWord, int idx) {
        List<List<string>> result = new List<List<string>>();

        // On the first iteration, the idx would be 0,
        // and you'd look for morphemes having the same size as the resultword.
        // Such morphemes don't exist. So subtract 1 from morphemeLength when idx is 0.
        for (int morphemeLength = resultWord.Length - (idx == 0 ? 1 : idx); morphemeLength > 0; morphemeLength--) {
            foreach (string morpheme in morphemeFinder.findMorphemes(morphemeLength, resultWord[idx])) {
                // only one morpheme will actually fit
                // however, there might be multiple combos follwing this one morpheme
                if (morphemeFits(morpheme, resultWord, idx)) {
                    if (idx + morpheme.Length == resultWord.Length) {
                        // base case
                        result.Add(new List<string>{morpheme});
                    } else {
                        List<List<string>> followingCombos = findCombosRec(resultWord, idx + morpheme.Length);
                        foreach (List<String> combo in followingCombos) {
                            if (combo.Count > 0) {
                                combo.Insert(0, morpheme);
                                result.Add(combo);
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

    public bool morphemeFits(string morpheme, string resultWord, int idx) {
        return new Regex("^" + morpheme).IsMatch(resultWord.Substring(idx));
    }
}