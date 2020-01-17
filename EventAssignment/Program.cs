using System;
using System.Collections.Generic;

namespace EventAssignment
{
      class Program
    {
        public static Action AssignCustomerTalbe;
        static void Main(string[] args)
        {
            Table table = new Table();
            Queue<Customer> CustQueue = new Queue<Customer>();
            CustQueue.Enqueue(new Customer("Joe", "Smith"));
            CustQueue.Enqueue(new Customer("Jane", "Jones"));
            CustQueue.Enqueue(new Customer("Jack", "Jump"));
            CustQueue.Enqueue(new Customer("Jeff", "Run"));
            CustQueue.Enqueue(new Customer("Jill", "Hill"));
            CustQueue.Enqueue(new Customer("John", "Winstone"));

            AssignCustomerTalbe = () =>
            {
                if (CustQueue.Count > 0)
                {
                    table.HaveNewCustomer(null, new HandleEventArgs(CustQueue.Dequeue(), table));
                }
                else
                {
                    Console.WriteLine("Everyone is Full!");
                    //return;
                }
            };
            AssignCustomerTalbe();
        }
    }

    public class Customer
    {
        public string FirstName;
        public string LastName;

        public Customer(string FN, string LN)
        {
            FirstName = FN;
            LastName = LN;
        }
        public enum Meals : int
        {
            none = 0,
            appetizer = 1,
            main = 2,
            desert = 3,
            done = 4

        }

        public void GetTable(object sender)
        {
            Console.WriteLine("{0} {1} got a table.", this.FirstName, this.LastName);
        }


        public void Changemeal(object sender, HandleEventArgs eventArgs)
        {
            foreach (string meal in Enum.GetNames(typeof(Customer.Meals)))
            {
                if (meal.Equals("done"))
                {
                    Console.WriteLine("{0} {1} is having {2}.", this.FirstName, this.LastName, meal);
                    Program.AssignCustomerTalbe();
                }
                else if (meal.Equals("none"))
                {
                    continue;
                }

                else
                {
                    Console.WriteLine("{0} {1} is having {2}.", this.FirstName, this.LastName, meal);
                }

            }

        }
    }

    public class Table
    {
        private bool IsOpen = true;
        public Customer CurrentCustomer = null;
        public event EventHandler<HandleEventArgs> HaveMeal;
        public event EventHandler<HandleEventArgs> AssignCustomerTable;
        public void HaveNewCustomer(object sender, HandleEventArgs eventArgs)
        {
            IsOpen = true;
            Console.WriteLine("Table is open!");
            AssignCustomerTable = SetTableOccupied;
            SetTableOccupied(this, eventArgs);
        }


        public void SetTableOccupied(Object sender, HandleEventArgs eventArgs)
        {
            CurrentCustomer = eventArgs.customer;
            Console.WriteLine("{0} {1} got a table.", eventArgs.customer.FirstName, eventArgs.customer.LastName);
            IsOpen = false;
            HaveMeal = CurrentCustomer.Changemeal;
            HaveMeal(this, new HandleEventArgs(CurrentCustomer, this));
        }

        public bool CheckTableStatus()
        {
            return IsOpen;
        }
    }


    public class HandleEventArgs : EventArgs

    {
        private Customer _CurrentCustomer;
        private Table _OccupiedTable;

        public Customer customer
        {
            get
            { return _CurrentCustomer; }
            set
            { _CurrentCustomer = value; }
        }

        public Table OccupiedTable
        {
            get
            { return _OccupiedTable; }
            set
            { _OccupiedTable = value; }
        }

        public HandleEventArgs(Customer customer, Table table)
        {
            _CurrentCustomer = customer;
            _OccupiedTable = table;
        }
    }
}
