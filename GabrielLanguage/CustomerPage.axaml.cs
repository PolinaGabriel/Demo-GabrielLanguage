using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using GabrielLanguage.Folder;
using GabrielLanguage.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GabrielLanguage;

public partial class CustomerPage : Window
{
    public string _photo;
    
    public CustomerPage()
    {
        InitializeComponent();
        tagList.ItemsSource = Helper.Database.Tags.ToList();
    }

    public string FillCheck()
    {
        string check = "";
        if (lastNameBox.Text == "" || firstNameBox.Text == "" || patronymBox.Text == "" || phoneBox.Text == "" || emailBox.Text == "")
        {
            check = "Все поля должны быть заполнены\n";
        }
        return check;
    }

    public string NameCheck(string name)
    {
        string check = "";
        List<string> forName = new List<string>();
        for (int i = 0; i < name.Length; i++)
        {
            forName.Add(name[i].ToString());
        }
        foreach (string s in forName)
        {
            if (s.ToLower() != "а" && s.ToLower() != "б" && s.ToLower() != "в" && s.ToLower() != "г" && s.ToLower() != "д" && s.ToLower() != "е" && s.ToLower() != "ё" &&
                s.ToLower() != "ж" && s.ToLower() != "з" && s.ToLower() != "и" && s.ToLower() != "й" && s.ToLower() != "к" && s.ToLower() != "л" && s.ToLower() != "м" &&
                s.ToLower() != "н" && s.ToLower() != "о" && s.ToLower() != "п" && s.ToLower() != "р" && s.ToLower() != "с" && s.ToLower() != "т" && s.ToLower() != "у" &&
                s.ToLower() != "ф" && s.ToLower() != "х" && s.ToLower() != "ц" && s.ToLower() != "ч" && s.ToLower() != "ш" && s.ToLower() != "щ" && s.ToLower() != "ъ" &&
                s.ToLower() != "ы" && s.ToLower() != "ь" && s.ToLower() != "э" && s.ToLower() != "ю" && s.ToLower() != "я" && s != "-" && s != " ")
            {
                check = "Некорректное ФИО\n";
                break;
            }
        }
        return check;
    }

    public string AllNamesCheck()
    {
        string check1 = NameCheck(lastNameBox.Text);
        string check2 = NameCheck(firstNameBox.Text);
        string check3 = NameCheck(patronymBox.Text);

        if (check1 != "" || check2 != "" || check3 != "")
        {
            if (check1 != "")
            {
                return check1;
            }
            else if (check2 != "")
            {
                return check2;
            }
            else
            {
                return check3;
            }
        }
        else
        {
            return "";
        }
    }

    public string PhoneCheck()
    {
        string check = "";
        List<string> forPhone = new List<string>();
        for (int i = 0; i < phoneBox.Text.Length; i++)
        {
            forPhone.Add(phoneBox.Text[i].ToString());
        }
        foreach (string s in forPhone)
        {
            if (s != "0" && s != "1" && s != "2" && s != "3" && s != "4" && s != "5" && s != "6" && s != "7" && s != "8" && s != "9" && s != "+" && s != "-" && s != "(" && s != ")" && s != " ")
            {
                check = "Некорректный телефон\n";
                break;
            }
        }
        return check;
    }

    public string EmailCheck()
    {
        string check = "";
        int at = -1;
        int ats = 0;
        int dot = -1;
        int dots = 0;
        List<string> forEmail = new List<string>();
        for (int i = 0; i < emailBox.Text.Length; i++)
        {
            forEmail.Add(emailBox.Text[i].ToString());
        }
        foreach (string s in forEmail)
        {
            if (s == ".")
            {
                dot = forEmail.IndexOf(s);
                dots++;
            }
            else if (s == "@")
            {
                at = forEmail.IndexOf(s);
                ats++;
            }
        }
        if (dot < at || dots == 0 || dots > 1 || ats == 0 || ats > 1 || dot == 0 || dot == forEmail.Count - 1 || at == 0 || at == forEmail.Count - 1)
        {
            check = "Некорректный emial\n";
        }
        return check;
    }

    private readonly FileDialogFilter fileFilter = new()
    {
        Extensions = new List<string>() { "png", "jpg", "jpeg" }
    };

    private async void SelectImage(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(fileFilter);
            var result = await dialog.ShowAsync(this);
            string file = String.Join("", result);
            long length = new System.IO.FileInfo(file).Length;
            if (length <= 20000)
            { 
                Random random = new Random();
                _photo = "Asset/photo" + random.Next(1, 10000) + ".jpg";
                System.IO.File.Copy(file, _photo);
                photoSample.Source = new Bitmap(_photo);
            }
            else
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.FillErrors("Размер фото не должен превышать 2 Мб");
                errorMessage.Show();
            }
        }
        catch { }
    }

    public void OpenCustomerVisits(object sender, RoutedEventArgs routedEventArgs)
    {
        CustomerVisits customerVisits = new CustomerVisits(Convert.ToInt32(idBlock.Text));
        customerVisits.Show();
    }

    public void Cancel(object sender, RoutedEventArgs routedEventArgs)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    public void Editing()
    {
        if (SexSwitch.IsChecked == false)
        {
            Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).SexId = 1;
        }
        else
        {
            Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).SexId = 2;
        }
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Lastname = lastNameBox.Text;
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Firstname = firstNameBox.Text;
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Patronym = patronymBox.Text;
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Birthdate = DateOnly.FromDateTime(birthCalendar.SelectedDate ?? DateTime.Now);
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Phone = phoneBox.Text;
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Email = emailBox.Text;
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Photo = _photo;
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Firstdate = DateOnly.FromDateTime(DateTime.Now);
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Lastdate = null;
        Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)).Visits = 0;
    }

    public Customer Adding(Customer customer)
    {
        if (SexSwitch.IsChecked == false)
        {
            customer.SexId = 1;
        }
        else
        {
            customer.SexId = 2;
        }
        customer.Lastname = lastNameBox.Text;
        customer.Firstname = firstNameBox.Text;
        customer.Patronym = patronymBox.Text;
        customer.Birthdate = DateOnly.FromDateTime(birthCalendar.SelectedDate ?? DateTime.Now);
        customer.Phone = phoneBox.Text;
        customer.Email = emailBox.Text;
        customer.Photo = _photo;
        customer.Firstdate = DateOnly.FromDateTime(DateTime.Now);
        customer.Lastdate = null;
        customer.Visits = 0;
        return customer;
    }

    public void RemovingTags()
    {
        foreach (CustomerTag customerTag in Helper.Database.CustomerTags)
        {
            if (customerTag.CustomerId == Convert.ToInt32(idBlock.Text))
            {
                Helper.Database.CustomerTags.Remove(customerTag);
            }
        }
    }

    public void AddingTags(Customer customer)
    {
        foreach (Tag tag in tagList.SelectedItems)
        {
            CustomerTag customerTag = new CustomerTag()
            {
                TagId = tagList.Items.IndexOf(tag) + 1,
                CustomerId = customer.Id
            };
            Helper.Database.CustomerTags.Add(customerTag);
        }
    }

    public void Save(object sender, RoutedEventArgs routedEventArgs)
    {
        string message = "";
        if (FillCheck() == "")
        {
            message = AllNamesCheck() + PhoneCheck() + EmailCheck();
        }
        else
        {
            message = FillCheck();
        }
        if (message == "")
        {
            if (this.Title == "Редактирование клиента")
            {
                Editing();
                RemovingTags();
                AddingTags(Helper.Database.Customers.FirstOrDefault(x => x.Id == Convert.ToInt32(idBlock.Text)));
            }
            else
            {
                Customer customer = new Customer();
                Helper.Database.Customers.Add(Adding(customer));
                AddingTags(customer);
            }
            Helper.Database.SaveChanges();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        else
        {
            ErrorMessage errorMessage = new ErrorMessage();
            errorMessage.FillErrors(message);
            errorMessage.Show();
        }
    }
}