using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;

namespace Lychee.Scrapper.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IScrappedSettingRepository _scrappedSettingRepository;

        //public SettingsController(IScrappedSettingRepository scrappedSettingRepository)
        //{
        //    _scrappedSettingRepository = scrappedSettingRepository;
        //}

        public ActionResult ScrapeSettings()
        {
            var model = _scrappedSettingRepository.GetAllSettings();
            return View(model);
        }

        public ActionResult ScrapeItemSettings(string category)
        {
            var model = _scrappedSettingRepository.GetItemSettings(category);
            ViewBag.Category = category;
            return View(model);
        }

        public ActionResult SystemSettings()
        {
            return View();
        }
    }
}