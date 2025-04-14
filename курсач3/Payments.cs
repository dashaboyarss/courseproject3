using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсач3
{
    internal class Payments :Plan
    {
        //public int time;
        //public double paymentAmount;

        public int Time
        {
            get => time;
            set
            {
                if (value >= 0) time = value * 4;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Срок достижения цели'! Введите целое неотрицательное число");
                    time = -1;
                    isCorrect = false;
                }

            }
        }

        public Payments(string name, int goalAmount, double startAmount, string frequency, double incomePercent, double investIncome, int time, double inflation) : base(name, goalAmount, startAmount, frequency, incomePercent, investIncome, inflation)
        {
            this.Time = time;
        }

        public void NewGoalAmount()
        {
            double years = this.Time / 12 / 4;
            double months = this.Time / 4 % 12;
            double newTargetSum = GoalAmount;
            if (newTargetSum == -1) this.amountWithInflation = -1;
            else
            {
                for (int i = 0; i < years; i++)
                {
                    newTargetSum = newTargetSum + (newTargetSum * inflation);
                }
                newTargetSum = newTargetSum + (newTargetSum * inflation / 12 * months);
                this.amountWithInflation = newTargetSum;
            }
        }

        public void InvestIncome()
        {
            if (this.IncomePercent == -1 || this.InvestAmount == -1 || this.Time == -1)
            {
                this.investIncome = -1;
            }
            else
            {
                double startSum = this.InvestAmount;
                double years = this.Time / 4 / 12;
                double months = this.Time / 4 % 12;
                double newInvestAmount = this.InvestAmount;

                for (int i = 0; i < years; i++)
                {
                    newInvestAmount = newInvestAmount + (newInvestAmount * this.IncomePercent);
                }
                newInvestAmount = newInvestAmount + (newInvestAmount * this.IncomePercent / 12 * months);
                this.investIncome = newInvestAmount - startSum;
            }
        }

        public void PaymentAmount()
        {
            if (this.Frequency == "" || this.amountWithInflation == -1 || this.investIncome == -1 || this.StartAmount == -1)
            {
                this.paymentAmount = -1;
            }
            else
            {
                double payment = 0;
                double paymentSum = this.amountWithInflation - this.investIncome - this.StartAmount - this.InvestAmount;

                if (Frequency == "раз в неделю")
                {
                    payment = paymentSum / this.Time;
                }
                else if (Frequency == "раз в 2 недели")
                {
                    payment = paymentSum / this.Time * 2;
                }
                else if (Frequency == "раз в месяц")
                {
                    payment = paymentSum / this.Time * 4;
                }
                else if (Frequency == "раз в 3 месяца")
                {
                    //if (this.Time % 3 == 0) payment = paymentSum / (this.Time / 3);
                    /*else*/ payment = paymentSum / this.Time * 12;
                }
                else if (Frequency == "раз в полгода")
                {
                    //if (this.Time % 6 == 0) payment = paymentSum / (this.Time / 6);
                    /*else*/ payment = paymentSum / this.Time * 24;
                }
                else if (Frequency == "раз в год")
                {
                    //if (this.Time % 12 == 0) payment = paymentSum / (this.Time / 12);
                    /*else*/
                    payment = paymentSum / this.Time * 48 ;
                }
                this.paymentAmount = payment;
            }

        }
    }
}
