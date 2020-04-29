/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SVS
{
    public class SimpleVisualizer : MonoBehaviour
    {
        public LSystemGenerator lsystem;
        List<Vector3> positions = new List<Vector3>();
        public GameObject prefab;
        public Material lineMaterial;

        private int length = 8;
        private float angle = 90;

        public int Length
        {
            get
            {
                if (length > 0)
                {
                    return length;
                }
                else
                {
                    return 1;
                }
            }
            set => length = value;
        }

        private void Start()
        {
            var sequence = lsystem.GenerateSentence();
            VisualizeSequence(sequence);
        }

        private void VisualizeSequence(string sequence)
        {
            Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
            var currentPosition = Vector3.zero;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = Vector3.zero;

            positions.Add(currentPosition);

            foreach (var letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters)letter;
                switch (encoding)
                {
                    case EncodingLetters.save:
                        savePoints.Push(new AgentParameters
                        {
                            position = currentPosition,
                            direction = direction,
                            length = Length
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count > 0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Length = agentParameter.length;
                        }
                        else
                        {
                            throw new System.Exception("Dont have saved point in our stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * length;
                        DrawLine(tempPosition, currentPosition, Color.red);
                        Length -= 2;
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;
                }
            }

            foreach (var position in positions)
            {
                Instantiate(prefab, position, Quaternion.identity);
            }

        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            GameObject line = new GameObject("line");
            line.transform.position = start;
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, end);
            lineRenderer.SetPosition(1, start);
        }

        public enum EncodingLetters
        {
            unknown = '1',
            save = '[',
            load = ']',
            draw = 'F',
            turnRight = '+',
            turnLeft = '-'
        }
    }
}


