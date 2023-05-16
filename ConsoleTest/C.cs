using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleTest;

    public class C
    {
        public void Interpret(string code)
        {
            var stack = new Stack<string>();
            var variables = new Dictionary<string, object>();
            var tokens = Tokenize(code);

            foreach (var token in tokens)
            {
                switch (token)
                {
                    case "int":
                    case "string":
                    case "bool":
                    case "double":
                    case "float":
                        stack.Push(token);
                        break;
                    case "=":
                        var value = stack.Pop();
                        var variable = stack.Pop();
                        variables[variable] = value;
                        break;
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        var b = stack.Pop();
                        var a = stack.Pop();
                        var result = Calculate(a, b, token);
                        stack.Push(result.ToString());
                        break;
                    case "print":
                        var print = stack.Pop();
                        Console.WriteLine(print);
                        break;
                    default:
                        if (variables.ContainsKey(token))
                        {
                            stack.Push(variables[token].ToString());
                        }
                        else
                        {
                            stack.Push(token);
                        }
                        break;
                }
            }
            
        }
        
        public int Calculate(string a, string b, string op)
        {
            switch (op)
            {
                case "+":
                    return int.Parse(a) + int.Parse(b);
                case "-":
                    return int.Parse(a) - int.Parse(b);
                case "*":
                    return int.Parse(a) * int.Parse(b);
                case "/":
                    return int.Parse(a) / int.Parse(b);
                default:
                    return 0;
            }
        }
        
        /// <summary>
        /// Tokenizes the input string and returns the tokens as a string array.
        /// </summary>
        /// <param name="input"></param>
        public string[] Tokenize(string input)
        {
            string pattern = @"\b(int|void|char|double|float|struct|if|else|switch|case|break|default|while|for|do|return)\b|[+-/*%=<>!&|~^]|[A-Za-z_][A-Za-z0-9_]*|[0-9]*\.?[0-9]+";
            
            var regex = new Regex(pattern);
            return regex.Matches(input)
                        .Select(m => m.Value)
                        .ToArray();
        }
        
        private bool IsKeyword(string token)
        {
            string[] keywords = { "int", "void", "char", "double", "float", "struct", "if", "else", "switch", "case", "break", "default", "while", "for", "do", "return" };
            return keywords.Contains(token);
        }

        private bool IsOperator(string token)
        {
            string[] operators = { "+", "-", "*", "/", "%", "=", "<", ">", "!", "&", "|", "~", "^" };
            return operators.Contains(token);
        }

        private bool IsVariable(string token)
        {
            // check if token is a valid variable name
            // by checking if it starts with a letter or underscore,
            // followed by zero or more letters, underscores, or digits
            return Regex.IsMatch(token, @"^[A-Za-z_][A-Za-z0-9_]*$");
        }

        private bool IsLiteral(string token)
        {
            // check if token is a literal value by
            // trying to parse it as a float or an integer
            return float.TryParse(token, out _) || int.TryParse(token, out _);
        }        
    }