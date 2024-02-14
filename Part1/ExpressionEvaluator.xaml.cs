using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Part1
{
    /// <summary>
    /// Interaction logic for ExpressionEvaluator.xaml
    /// </summary>
    public partial class ExpressionEvaluator : Window
    {
        public ExpressionEvaluator()
        {
            InitializeComponent();
            TrickButton.Content = "->";
            FirstValue.Text = "1 + 2 * 3";
        }

        private void TrickButton_Click(object sender, RoutedEventArgs e)
        {
            var evaluatedValue = EvaluateExpression(FirstValue.Text);
            var result = string.Empty;
            if (!double.IsNaN(evaluatedValue))
                result = evaluatedValue.ToString(CultureInfo.InvariantCulture);
            SecondValue.Text = result;
        }

        private static double EvaluateExpression(string expression)
        {
            var tokens = expression.Split(' ');
            var numbers = new Stack<double>();
            var operators = new Stack<char>();

            foreach (var token in tokens)
                if (double.TryParse(token, out var number))
                {
                    numbers.Push(number);
                }
                else if (IsOperator(token[0]))
                {
                    while (operators.Count > 0 && Priority(operators.Peek()) >= Priority(token[0]))
                    {
                        var operand2 = numbers.Pop();
                        var operand1 = numbers.Pop();
                        var op = operators.Pop();
                        numbers.Push(ApplyOperation(operand1, operand2, op));
                    }

                    operators.Push(token[0]);
                }
                else
                {
                    return double.NaN;
                }

            while (operators.Count > 0)
            {
                var operand2 = numbers.Pop();
                var operand1 = numbers.Pop();
                var op = operators.Pop();
                numbers.Push(ApplyOperation(operand1, operand2, op));
            }

            return numbers.Pop();
        }

        private static bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/';
        }

        private static int Priority(char op)
        {
            switch (op)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                default:
                    return 0;
            }
        }

        private static double ApplyOperation(double operand1, double operand2, char op)
        {
            switch (op)
            {
                case '+':
                    return operand1 + operand2;
                case '-':
                    return operand1 - operand2;
                case '*':
                    return operand1 * operand2;
                case '/':
                    if (operand2 != 0)
                        return operand1 / operand2;
                    return double.NaN;
                default:
                    return double.NaN;
            }
        }
    }
}
