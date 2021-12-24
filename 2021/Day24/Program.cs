using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            ArithmeticLogicUnit alu = new ArithmeticLogicUnit();

            List<Variable> variables = new List<Variable>()
            {
                new Variable("w", 0),
                new Variable("x", 0),
                new Variable("y", 0),
                new Variable("z", 0) 
            };

            alu.LoadProgram(args[0]);
            alu.SetVariables(variables);
            alu.Run("13579246899999");

            Variable answer = variables.Where(var => var.Name == "z").First();

            Console.WriteLine($"Answer 1: {answer.Value}");
        }
    }

    class ArithmeticLogicUnit
    {
        private List<string> instructions;
        private List<Variable> variables;
        private Dictionary<string, Operation> operations;
        private delegate void Operation(Variable variable, int value);

        public ArithmeticLogicUnit()
        {
            operations = new Dictionary<string, Operation>()
            {
                { "inp", Input },
                { "add", Add },
                { "mul", Multiply },
                { "div", Divide }, 
                { "mod", Modulo },
                { "eql", Equal }
            };
        }

        public void LoadProgram(string fileName)
        {
            instructions = new List<string>(File.ReadLines(fileName));
        }

        public void SetVariables(List<Variable> variables)
        {
             this.variables = variables;
        }

        public void Run(string input)
        {
            if(instructions != null && instructions.Count > 0)
            {
                Queue<int> inputs = new Queue<int>(input.Select(imp => Int32.Parse(imp.ToString())));

                foreach (string instruction in instructions)
                {
                    Console.WriteLine($"Performing {instruction}");

                    string[] operands = instruction.Split(" ");

                    Operation operation = GetOperation(operands[0]);
                    Variable variable = GetVariable(operands[1]);
                    int value = operands.Length > 2 ? GetValue(operands[2]) : inputs.Dequeue();
                    
                    operation(variable, value);
                }
            }
            else
            {
                throw new InvalidOperationException("No instructions to execute");
            }
        }

        private int GetValue(string value)
        {
            return int.TryParse(value, out _ ) ? Int32.Parse(value) : GetVariable(value).Value;
        }

        private Operation GetOperation(string name)
        {
            if (!operations.ContainsKey(name))
            {
                throw new InvalidOperationException($"Operation \'{name}\' is not supported");
            }
            else
            {
                return operations[name];
            }
        }

        private Variable GetVariable(string name)
        {
            Variable variable = variables.Where(var => var.Name == name).First();

            if (variable == null)
            {
                throw new InvalidOperationException($"Variable \'{name}\' does not exist");
            }
            else
            {
                return variable;
            }
        }

        private void Input(Variable variable, int value) { variable.Value = value; }

        private void Add(Variable variable, int value) { variable.Value = variable.Value + value; }

        private void Multiply(Variable variable, int value) { variable.Value = variable.Value * value; }

        private void Divide(Variable variable, int value) { variable.Value = variable.Value / value; }

        private void Modulo(Variable variable, int value) { variable.Value = variable.Value % value; }

        private void Equal(Variable variable, int value) { variable.Value = variable.Value == value ? 1 : 0; }
    }

    class Variable
    {
        public int Value { get; set; }
        public string Name { get; }

        public Variable(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() { return $"{Name}={Value}"; }
    }
}
