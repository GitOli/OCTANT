using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentitySample.Models;
using Interview;

namespace Octant.Models.Interview
{
    //public class CampaignService : IDisposable
    //{
    //    private ApplicationDbContext entities = new ApplicationDbContext();

    //    public CampaignService(ApplicationDbContext entities)
    //    {
    //        this.entities = entities;
    //    }

    //    public IEnumerable<CampaignViewModel> Read()
    //    {
    //        return entities.Campaigns.Select(Campaign => new CampaignViewModel
    //        {
    //            IdCampaign = Campaign.IdCampaign,
    //            Name = Campaign.Name,
    //            StartDate = Campaign.StartDate,
    //            EndDate = Campaign.EndDate,
    //            Status = Campaign.Status,
    //            IdCustomer = Campaign.IdCustomer,
    //            Customer = new CustomerViewModel()
    //            {
    //                IdCustomer = Campaign.Customer.IdCustomer,
    //                Name = Campaign.Customer.Name
    //            },
    //            //IdFramework = Campaign.IdFramework,
    //            //Framework = new FrameworkViewModel()
    //            //{
    //            //    IdFramework = Campaign.Framework.IdFramework,
    //            //    Name = Campaign.Framework.Name
    //            //}
    //        });
    //    }

    //    public void Create(CampaignViewModel Campaign)
    //    {
    //        var entity = new Campaign();

    //        entity.Name = Campaign.Name;
    //        entity.StartDate = Campaign.StartDate;
    //        entity.EndDate = Campaign.EndDate;
    //        entity.Status = Campaign.Status;
    //        entity.IdCustomer = Campaign.IdCustomer;

    //        if (entity.IdCustomer == null)
    //        {
    //            entity.IdCustomer = 1;
    //        }

    //        if (Campaign.IdCustomer != null)
    //        {
    //            entity.IdCustomer = Campaign.Customer.IdCustomer;
    //        }

    //        entities.Campaigns.Add(entity);
    //        entities.SaveChanges();

    //        Campaign.IdCampaign = entity.IdCampaign;
    //    }

    //    public void Update(CampaignViewModel Campaign)
    //    {
    //        var entity = new Campaign();

    //        entity.IdCampaign = Campaign.IdCampaign;
    //        entity.Name = Campaign.Name;
    //        entity.StartDate = Campaign.StartDate;
    //        entity.EndDate = Campaign.EndDate;
    //        entity.Status = Campaign.Status;
    //        entity.IdCustomer = Campaign.IdCustomer;

    //        if (Campaign.Customer != null)
    //        {
    //            entity.IdCustomer = Campaign.Customer.IdCustomer;
    //        }

    //        entities.Campaigns.Attach(entity);
    //        entities.Entry(entity).State = EntityState.Modified;
    //        entities.SaveChanges();
    //    }

    //    public void Destroy(CampaignViewModel Campaign)
    //    {
    //        var entity = new Campaign();

    //        entity.IdCampaign = Campaign.IdCampaign;

    //        entities.Campaigns.Attach(entity);

    //        entities.Campaigns.Remove(entity);

    //        entities.SaveChanges();
    //    }

    //    public void Dispose()
    //    {
    //        entities.Dispose();
    //    }
//    }
}
