using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;

namespace conlib;

public class ParamReplacer
{
    public static string PrependLines(string rawLines, string prependText)
    {
        string result = "";
        int i = 0;
        int next_i = rawLines.IndexOf('\n');
        while (next_i != -1)
        {
            result = (
                result
                + prependText
                + rawLines.Substring(i, next_i - i + 1)
            );
            i = next_i + 1;
            next_i = rawLines.IndexOf('\n', i);
        }
        return result;
    }

    public static string ReplaceParams(
        string sourceText,
        Dictionary<string, string> ParamsDict
    )
    {
        string result = "";
        int? previousNewlineIndex = null;
        int previousInsertEndIndex = 0;
        int colonIndex = sourceText.IndexOf(':');
        while (colonIndex != -1)
        {
            if (sourceText.Substring(colonIndex, 7) != ":param ")
            {
                colonIndex = sourceText.IndexOf(':', colonIndex + 1);
                continue;
            }

            int newNewlineIndex;
            if (previousNewlineIndex == null) newNewlineIndex = sourceText.IndexOf('\n');
            else newNewlineIndex = sourceText.IndexOf('\n', (int)previousNewlineIndex + 1);

            while (newNewlineIndex < colonIndex)
            {
                previousNewlineIndex = newNewlineIndex;
                if (newNewlineIndex != -1) newNewlineIndex = sourceText.IndexOf('\n', (int)previousNewlineIndex + 1);
                else break;
            }

            int nextColonIndex = sourceText.IndexOf(':', colonIndex + 1);
            string potentialKey = sourceText.Substring(
                colonIndex, nextColonIndex - colonIndex + 1
            );
            string replaceValue;
            try
            {
                replaceValue = ParamsDict[potentialKey];
            }
            catch (KeyNotFoundException)
            {
                colonIndex = nextColonIndex + 1;
                continue;
            }

            result = (
                result
                + sourceText.Substring(previousInsertEndIndex, colonIndex - previousInsertEndIndex)
                + replaceValue
            );
            
            previousInsertEndIndex = (newNewlineIndex == -1)? sourceText.Length: newNewlineIndex + 1;
            colonIndex = sourceText.IndexOf(':', previousInsertEndIndex);
        }
        result = result + sourceText.Substring(previousInsertEndIndex);
        return result;
    }
}

public class ParamLoader
{
    public static Dictionary<string, string> GetParamDict(string paramFileText)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        paramFileText = paramFileText.Replace("\r\n", "\n");
        int i = paramFileText.IndexOf(':');
        while (i != -1)
        {
            if (
                paramFileText.Substring(i, 7) == ":param "
            )
            {
                int closingColonIndex = paramFileText.IndexOf(':', i + 7);

                if (closingColonIndex == -1) return result;
                int newKeyColonIndex = paramFileText.IndexOf(
                    ':',
                    closingColonIndex + 1
                );

                if (newKeyColonIndex == -1)
                {
                    result.Add(
                        paramFileText.Substring(i, closingColonIndex - i + 1),
                        paramFileText.Substring(i)
                    );
                    return result;
                }
                else
                {
                    result.Add(
                        paramFileText.Substring(i, closingColonIndex - i + 1),
                        paramFileText.Substring(
                            i,
                            newKeyColonIndex - i
                        )
                    );
                    i = newKeyColonIndex;
                }

            }
            else i = paramFileText.IndexOf(':', i + 1);
        }
        return result;
    }
}