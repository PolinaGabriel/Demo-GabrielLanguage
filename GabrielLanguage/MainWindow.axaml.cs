using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using GabrielLanguage.Folder;
using GabrielLanguage.Models;
using Microsoft.EntityFrameworkCore;
using Avalonia.Media.Imaging;
using Tag = GabrielLanguage.Models.Tag;

namespace GabrielLanguage
{
    public partial class MainWindow : Window
    {
        public List<Customer> _customers = new List<Customer>();
        public List<Customer> _page = new List<Customer>();
        public List<Tag> _allTags = Helper.Database.Tags.ToList();
        public int _onPage;
        public int _firstIndex;
        public int _seen;
        
        public MainWindow()
        {
            InitializeComponent();
            searchBox.Text = "";
            filterBox.SelectedIndex = 0;
            sortBox.SelectedIndex = 0;
            birthMonth.IsChecked = false;
            all.IsChecked = true;
            _firstIndex = 0;
            _onPage = Helper.Database.Customers.Count();
            _seen = Helper.Database.Customers.Count();
            Fill();
            if (_customers.Count < 10)
            {
                ten.IsEnabled = false;
                fifty.IsEnabled = false;
                hundred.IsEnabled = false;
            }
            else if (_customers.Count >= 10 && _customers.Count < 50)
            {
                ten.IsEnabled = true;
                fifty.IsEnabled = false;
                hundred.IsEnabled = false;
            }
            else if (_customers.Count >= 50 && _customers.Count < 200)
            {
                ten.IsEnabled = true;
                fifty.IsEnabled = true;
                hundred.IsEnabled = false;
            }
            else
            {
                ten.IsEnabled = true;
                fifty.IsEnabled = true;
                hundred.IsEnabled = true;
            }
            all.IsEnabled = true;
            all.IsChecked = true;
        }

        public void Fill()
        {
            _page.Clear();
            _customers = Helper.Database.Customers.Include(x => x.CustomerTags).Include(x => x.Sex).ToList();
            _customers = ForSort(ForSearch(ForFilter(ForCheck(_customers))));
            for (int i = 0; i < _onPage; i++)
            {
                _page.Add(_customers[i + _firstIndex]);
                if (i + _firstIndex == _customers.Count() - 1)
                {
                    break;
                }
            }
            List<Tag> tags = Helper.Database.Tags.ToList();
            customerList.ItemsSource = _page.ToList().Select(c => new
            {
                c.Id,
                c.Sex.SexName,
                c.Lastname,
                c.Firstname,
                c.Patronym,
                c.Birthdate,
                c.Phone,
                c.Email,
                c.Firstdate,
                c.Lastdate,
                c.Photo,
                c.Visits,
                Tags = c.CustomerTags.Select(t => new
                {
                    tags[t.TagId - 1].TagName,
                    tags[t.TagId - 1].Color               
                })
            });
            number.Text = _seen + " / " + _customers.Count();
            EnablingArrows();
        }

        public List<Customer> ForSort(List<Customer> customers)
        {
            switch (sortBox.SelectedIndex)
            {
                default:
                    return customers;

                case 1:
                    return customers.OrderBy(c => c.Lastname).ToList();

                case 2:
                    return customers.OrderByDescending(c => c.Lastdate).ToList();

                case 3:
                    return customers.OrderByDescending(c => c.Visits).ToList();
            }
        }

        public void Sort(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (_onPage != 0)
            {
                _firstIndex = 0;
                Fill();
            }
        }

        public List<Customer> ForSearch(List<Customer> customers)
        {
            switch (searchBox.Text)
            {
                case "":
                    return customers;

                default:
                    IEnumerable<Customer> search =
                    from c in customers
                    where c.Lastname.ToLower().Contains(searchBox.Text.ToLower()) ||
                          c.Firstname.ToLower().Contains(searchBox.Text.ToLower()) ||
                          c.Patronym.ToLower().Contains(searchBox.Text.ToLower()) ||
                          c.Phone.Contains(searchBox.Text) ||
                          c.Email.ToLower().Contains(searchBox.Text.ToLower())
                    select c;
                    return search.ToList();
            }
        }

        public void Search(object sender, KeyEventArgs keyEventArgs)
        {
            if (_onPage != 0)
            {
                _firstIndex = 0;
                Fill();
            }
        }

        public List<Customer> ForFilter(List<Customer> customers)
        {
            switch (filterBox.SelectedIndex)
            {
                case 0:
                    return customers;

                default:
                    IEnumerable<Customer> filter =
                    from c in customers
                    where c.SexId == filterBox.SelectedIndex
                    select c;
                    return filter.ToList();
            }
        }

        public void Filter(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (_onPage != 0)
            {
                _firstIndex = 0;
                Fill();
            }
        }

        public List<Customer> ForCheck(List<Customer> customers)
        {
            switch (birthMonth.IsChecked)
            {
                default:
                    return customers;

                case true:
                    IEnumerable<Customer> check =
                    from c in customers
                    where c.Birthdate.Month == DateTime.Now.Month
                    select c;
                    return check.ToList();
            }
        }

        public void Check(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_onPage != 0)
            {
                _firstIndex = 0;
                Fill();
            }
        }

        public void EnablingArrows()
        {
            if (all.IsChecked == true)
            {
                forward.IsEnabled = false;
                back.IsEnabled = false;
            }
            else if (_firstIndex == 0 && all.IsChecked == false)
            {
                forward.IsEnabled = true;
                back.IsEnabled = false;
            }
            else if (_page[_page.Count() - 1] == _customers[_customers.Count() - 1])
            {
                forward.IsEnabled = false;
                back.IsEnabled = true;
            }
            else
            {
                forward.IsEnabled = true;
                back.IsEnabled = true;
            }
        }

        public void OnPage(object sender, RoutedEventArgs routedEventArgs)
        {
            if (ten.IsChecked == true)
            {
                _onPage = 10;
            }
            else if (fifty.IsChecked == true)
            {
                _onPage = 50;
            }
            else if (hundred.IsChecked == true)
            {
                _onPage = 200;
            }
            else
            {
                _onPage = _customers.Count();
            }
            _seen = _onPage;
            number.Text = _seen + " / " + _customers.Count();
            _firstIndex = 0;
            Fill();
        }

        public void Forward(object sender, RoutedEventArgs routedEventArgs)
        {
            _firstIndex += _onPage;
            Fill();
            _seen += _page.Count();
            number.Text = _seen + " / " + _customers.Count();
        }

        public void Back(object sender, RoutedEventArgs routedEventArgs)
        {
            _firstIndex -= _onPage;
            _seen -= _page.Count();
            Fill();
        }

        public void EditCustomer(object sender, RoutedEventArgs routedEventArgs)
        {
            CustomerPage customerPage = new CustomerPage();
            customerPage.Title = "Редактирование клиента";
            customerPage._photo = Convert.ToString(Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Photo);
            customerPage.idBlock.Text = Convert.ToString(Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Id);
            customerPage.lastNameBox.Text = Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Lastname;
            customerPage.firstNameBox.Text = Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Firstname;
            customerPage.patronymBox.Text = Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Patronym;
            if (Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).SexId == 1)
            {
                customerPage.SexSwitch.IsChecked = false;
            }
            else
            {
                customerPage.SexSwitch.IsChecked = true;
            }
            customerPage.birthCalendar.SelectedDate = Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Birthdate.ToDateTime(TimeOnly.MinValue);
            customerPage.birthCalendar.DisplayDate = Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Birthdate.ToDateTime(TimeOnly.MinValue);
            customerPage.phoneBox.Text = Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Phone;
            customerPage.emailBox.Text = Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Email;
            customerPage.photoSample.Source = new Bitmap("Asset/" + Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag).Photo);
            List<CustomerTag> customerTags = Helper.Database.CustomerTags.Include(x => x.Tag).Where(c => c.CustomerId == Convert.ToInt32(customerPage.idBlock.Text)).ToList();
            customerPage.tagList.ItemsSource = _allTags.ToList();
            foreach (Tag tag in _allTags)
            {
                foreach (CustomerTag customerTag in customerTags)
                {
                    if (tag.Id == customerTag.TagId)
                    {
                        customerPage.tagList.SelectedItems.Add(tag);
                    }
                }
            }
            customerPage.Show();
            this.Close();
        }

        public void DeleteCustomer(object sender, RoutedEventArgs routedEventArgs)
        {
            Helper.Database.Customers.Remove(Helper.Database.Customers.FirstOrDefault(x => x.Id == (int)(sender as Button).Tag));
            IEnumerable<CustomerTag> remTags =
            from t in Helper.Database.CustomerTags
            where t.CustomerId == (int)(sender as Button).Tag
            select t;
            foreach (CustomerTag t in remTags)
            {
                Helper.Database.CustomerTags.Remove(t);
            }
            IEnumerable<Visit> remVisits =
            from v in Helper.Database.Visits
            where v.CustomerId == (int)(sender as Button).Tag
            select v;
            foreach (Visit v in remVisits)
            {
                Helper.Database.Visits.Remove(v);
            }
            Helper.Database.SaveChanges();
            _seen--;
            Fill();
        }

        public void NewCustomer(object sender, RoutedEventArgs routedEventArgs)
        {
            CustomerPage customerPage = new CustomerPage();
            customerPage.Title = "Новый клиент";
            customerPage._photo = "Asset/default.jpg";
            customerPage.idBlock.Text = "new";
            customerPage.lastNameBox.Text = "";
            customerPage.firstNameBox.Text = "";
            customerPage.patronymBox.Text = "";
            customerPage.phoneBox.Text = "";
            customerPage.emailBox.Text = "";
            customerPage.photoSample.Source = new Bitmap("Asset/default.jpg");
            customerPage.tagList.ItemsSource = _allTags.ToList();
            customerPage.Show();
            this.Close();
        }
    }
}