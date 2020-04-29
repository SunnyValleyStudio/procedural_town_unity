/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System.Text;
using UnityEngine;

namespace SVS
{
	public class LSystemGenerator : MonoBehaviour
	{
		public Rule[] rules;
		public string rootSentence;
		[Range(0,10)]
		public int iterationLimit = 1;

		public bool randomIgnoreRuleModifier = true;
		[Range(0, 1)]
		public float chanceToIgnoreRule = 0.3f;

		private void Start()
		{
			Debug.Log(GenerateSentence());
		}

		public string GenerateSentence(string word = null)
		{
			if(word == null)
			{
				word = rootSentence;
			}
			return GrowRecursive(word);
		}

		private string GrowRecursive(string word, int iterationIndex = 0)
		{
			if(iterationIndex >= iterationLimit)
			{
				return word;
			}
			StringBuilder newWord = new StringBuilder();

			foreach (var c in word)
			{
				newWord.Append(c);
				ProcessRulesRecursivelly(newWord, c, iterationIndex);
			}

			return newWord.ToString();
		}

		private void ProcessRulesRecursivelly(StringBuilder newWord, char c, int iterationIndex)
		{
			foreach (var rule in rules)
			{
				if(rule.letter == c.ToString())
				{
					if (randomIgnoreRuleModifier && iterationIndex > 1)
					{
						if(Random.value < chanceToIgnoreRule)
						{
							return;
						}
					}
					newWord.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
				}
				
			}
		}
	}
}

