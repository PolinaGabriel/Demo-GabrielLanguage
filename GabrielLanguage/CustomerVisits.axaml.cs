using Avalonia.Controls;
using GabrielLanguage.Models;
using System.Collections.Generic;
using GabrielLanguage.Folder;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GabrielLanguage;

public partial class CustomerVisits : Window
{
    public List<File> _files = Helper.Database.Files.ToList();
    public List<Visit> _visits = new List<Visit>();
    public int _id;

    public CustomerVisits(int id)
    {
        InitializeComponent();
        _id = id;
        Fill();
    }

    public void Fill()
    {
        _visits = Helper.Database.Visits.Where(v => v.CustomerId == _id).Include(v => v.VisitFiles).ToList();
        visitList.ItemsSource = _visits.ToList().Select(v => new
        {
            v.Date,
            v.Time,
            Files = v.VisitFiles.Select(f => new
            {
                _files[f.FileId].FileName
            })
        });
    }
}