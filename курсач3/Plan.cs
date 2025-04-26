using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace курсач3
{
    public class Plan
    {
        public string name;
        public double goalAmount;
        public double startAmount;
        public string frequency;
        public double incomePercent;
        public double investAmount;
        public double inflation;
        public double amountWithInflation;
        public double investIncome;
        public int time;
        public double paymentAmount;
        public int countPayments;
        public bool isCorrect = true;


        public string Name
        {
            get => name;
            set => name = value;
        }


        public double GoalAmount
        {
            get => goalAmount;
            set
            {
                if (value > 0)
                {
                    goalAmount = value;
                }
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Сумма цели'! Введите неотрицательное целое число");
                    goalAmount = -1;
                    isCorrect = false;
                }
            }
        }
        public double StartAmount
        {
            get=> startAmount;
            set
            {
                if (value <= goalAmount && value >= 0)
                {
                    startAmount = value;
                }
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Начальная сумма'! Введите неотрицательное целое число меньше значения поля 'Сумма цели'");
                    startAmount = -1;
                    isCorrect = false;
                }
            }
        }
        public string Frequency
        {
            get => frequency;
            set
            {
                if (value == "раз в неделю" || value == "раз в 2 недели" || value == "раз в месяц" || value == "раз в 3 месяца" || value == "раз в полгода" || value == "раз в год")
                {
                    frequency = value;
                }
                else
                {
                    MessageBox.Show("Некорректно заполнено поле частоты взносов! Выберите один из предложенных вариантов внесения взносов");
                    frequency = "";
                    isCorrect = false;
                }
            }
        }

        public double IncomePercent
        {
            get => incomePercent;
            set
            {
                if (value >= 0 && value <= 1) incomePercent = value ;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Ожидаемая доходность от инвестиций'! Введите число от 0 до 100");
                    incomePercent = -1;
                    isCorrect = false;
                }
            }
        }

        public double InvestAmount
        {
            get => investAmount;
            set
            {
                if (value >= 0) investAmount = value;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Сумма на инвестиционном счету'! Введите целое неотрицательное число");
                    investAmount = -1;
                    isCorrect = false;
                }
            }
        }


        public double Inflation
        {
            get => inflation;
            set
            {
                if (value >= 0 && value <= 1) inflation = value;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Текущий уровень инфляции'! Введите число от 0 до 100");
                    inflation = -1;
                    isCorrect = false;
                }
            }
        }

        public Plan(string name, int goalAmount, double startAmount, string frequency, double incomePercent, double investAmount,/*int time,*/ double inflation)
        {
            this.name = name;
            this.goalAmount = goalAmount;
            this.StartAmount = startAmount;
            this.Frequency = frequency;
            this.IncomePercent = incomePercent;
            this.investAmount = investAmount;
            this.Inflation = inflation;
        }

        public Plan(string name, double goalAmount, double startAmount, string frequency, double incomePercent, double investAmount, double inflation, double amountWithInflation, double investIncome, 
            int time, double paymentAmount, int countPayments)
        {
            this.name = name;
            this.goalAmount = goalAmount;
            this.StartAmount = startAmount;
            this.Frequency = frequency;
            this.IncomePercent = incomePercent;
            this.investAmount = investAmount;
            this.Inflation = inflation;
            this.amountWithInflation = amountWithInflation;
            this.investIncome = investIncome;
            this.time = time;
            this.paymentAmount = paymentAmount;
            this.countPayments = countPayments;
        }


        public bool CheckSum()
        {
            bool result = true;
            if (this.GoalAmount <= this.StartAmount + this.InvestAmount)
            {
                MessageBox.Show("Значение поля 'Сумма цели' должна превышать сумму значений полей 'Начальная сумма без инвестиций' и 'Сумма на инвестиционном счету'");
                result = false;
            }
            return result;
        }

        public bool CheckTime()
        {
            bool result = true;
            if (this.frequency == "раз в 3 месяца")
            {
                if (this.time / 4 < 3)
                {
                    result = false;
                }
            }
            else if (this.frequency == "раз в полгода")
            {
                if (this.time / 4 < 6)
                {
                    result = false;
                }
            }
            else if (this.frequency == "раз в год")
            {
                if (this.time / 4 < 12)
                {
                    result = false;
                }
            }

            if (!result) MessageBox.Show("Срок цели не может быть достигнут ранее первого взноса!");
            return result;
        }

        public bool CheckDifference()
        {
            double startDifference = this.GoalAmount - this.StartAmount - this.InvestAmount;
            double goalAmountAfter = 0;
            double incomeAfter = 0;

            if (frequency == "раз в неделю")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 12 / 4;
                incomeAfter = this.investAmount * this.incomePercent / 12 / 4 + this.paymentAmount + this.StartAmount + this.InvestAmount;
            }
            else if (frequency == "раз в 2 недели")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 12 / 2;
                incomeAfter = this.investAmount * this.incomePercent / 12 / 2 + this.paymentAmount + this.StartAmount + this.InvestAmount;
            }
            else if (frequency == "раз в месяц")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 12 ;
                incomeAfter = this.investAmount * this.incomePercent / 12 + this.paymentAmount + this.StartAmount + this.InvestAmount;
            }
            else if (frequency == "раз в 3 месяца")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 4;
                incomeAfter = this.investAmount * this.incomePercent / 4 + this.paymentAmount + this.StartAmount + this.InvestAmount;
            }
            else if (frequency == "раз в полгода")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 2;
                incomeAfter = this.investAmount * this.incomePercent / 2 + this.paymentAmount + this.StartAmount + this.InvestAmount;
            }
            else if (frequency == "раз в год")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation;
                incomeAfter = this.investAmount * this.incomePercent + this.paymentAmount + this.StartAmount + this.InvestAmount;
            }

            if (goalAmountAfter - incomeAfter > startDifference)
            {
                MessageBox.Show("Взносы слишком малы. Сумма цели растет быстрее, чем увеличиваются накопления!");
                return false;
            }
            else return true;
        }

        public int CountPayments()
        {
            if (frequency == "раз в неделю")
            {
                this.countPayments = this.time;
            }
            else if (frequency == "раз в 2 недели")
            {
                this.countPayments = this.time / 2;
            }
            else if (frequency == "раз в месяц")
            {
                this.countPayments = this.time / 4;
            }
            else if (frequency == "раз в 3 месяца")
            {
                this.countPayments = this.time / 12;
            }
            else if (frequency == "раз в полгода")
            {
                this.countPayments = this.time / 24;
            }
            else
            {
                this.countPayments = this.time / 48;
            }
            return -1;
        }
    }
}