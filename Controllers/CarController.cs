using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace DriveForum.Controllers
{
    public class CarController : Controller
    {
        ApplicationContext _context;

        public CarController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("moderator")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> NewCar()
        {
            CarsForModerator carsForModerator = new()
            {
                Cars = await _context.Cars.Include(u => u.Model.Brand.CarModels).Include(u => u.Engine).ToListAsync(),
                Brands = await _context.CarBrands.Include(u => u.CarModels).ToListAsync(),
                Engines = await _context.CarEngines.ToListAsync()
            };
            //List<Car> cars = await _context.Cars.Include(u => u.Model.Brand.CarModels).Include(u => u.Engine).ToListAsync();
            return View(carsForModerator);
        }

        /*[HttpPost]
        public async Task<IActionResult> NewCar(int carbrandid, int carmodelid, int engineid)
        {
            var newcarbrand = _context.CarBrands.FindAsync(carbrandid);
            var newcarmodel = _context.CarModels.FindAsync(carmodelid);
            var newengine = _context.CarEngines.FindAsync(engineid);
            CarModel newcarmodel = new() { }
            Car newcar = new() { Engine = newengine, Model = }
            _context.Cars.Add()

            return View();
        }*/

        /*public async Task<IActionResult> NewBrand(string brandname, string country)
        {
            if (_context.CarBrands.Where(u=>u.Name == brandname).FirstOrDefault() == null)
            {
                CarBrand newcarbrand = new CarBrand { Name = brandname, Country = country, CarModels = new() };
                _context.CarBrands.Add(newcarbrand);
                await _context.SaveChangesAsync();
            }
            return View();
        }

        public async Task<IActionResult> NewModel(string brandname, string country)
        {
            if (_context.CarBrands.Where(u => u.Name == brandname).FirstOrDefault() == null)
            {
                CarBrand newcarbrand = new CarBrand { Name = brandname, Country = country, CarModels = new() };
                _context.CarBrands.Add(newcarbrand);
                await _context.SaveChangesAsync();
            }
            return View();
        }*/
        [HttpPost]
        [Route("moderator/add")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> NewCar(string brandname, string country, string modelname, string modelyear, string enginename, string enginecapacity)
        {
            if (brandname != null && country != null && modelname == null && modelyear == null && enginename == null && enginecapacity == null)
            {
                if ((_context.CarBrands.FirstOrDefault(b => b.Name == brandname && b.Country == country)) == null)
                {
                    CarBrand newCarBrand = new CarBrand() { Name = brandname, Country = country, CarModels = new() };
                    _context.CarBrands.Add(newCarBrand);
                    _context.SaveChanges();
                }
                else TempData["Error"] = "Внимание! Данная марка уже существует.";
            }
            else if (brandname == null && country == null && modelname == null && modelyear == null && enginename != null && enginecapacity != null)
            {
                float enginecapacityfloat = float.Parse(enginecapacity.Replace('.', ','));
                if ((_context.CarEngines.FirstOrDefault(e => e.Name == enginename && e.Capacity == enginecapacityfloat) == null))
                {
                    CarEngine newCarEngine = new CarEngine() { Name = enginename, Capacity = enginecapacityfloat };
                    _context.CarEngines.Add(newCarEngine);
                    _context.SaveChanges();
                }
                else TempData["Error"] = "Внимание! Данный двигатель уже существует.";
            }

            else if (brandname != null && country != null && modelname != null && modelyear != null && enginename == null && enginecapacity == null)
            {
                int modelyearint = int.Parse(modelyear);
                var existingBrand = _context.CarBrands.Include(u=>u.CarModels).FirstOrDefault(b => b.Name == brandname && b.Country == country);
                if (existingBrand != null)
                {
                    var existingModel = existingBrand?.CarModels?.FirstOrDefault(m => m.Name == modelname && m.Year == modelyearint);
                    if (existingModel == null)
                    {
                        CarModel newCarModel = new CarModel() { Name = modelname, Year = modelyearint, Brand = existingBrand };
                        _context.CarModels.Add(newCarModel);
                        _context.SaveChanges();
                    }
                    else TempData["Error"] = "Внимание! Данная модель у марки уже существует.";
                }
                else TempData["Error"] = "Внимание! Данной марки не существует, необходимо добавить.";
            }

            else if (brandname != null && country != null && modelname != null && modelyear != null && enginename != null && enginecapacity != null)
            {
                float enginecapacityfloat = float.Parse(enginecapacity.Replace('.', ','));
                int modelyearint = int.Parse(modelyear);
                CarBrand existingBrand = _context.CarBrands.Include(u=>u.CarModels).FirstOrDefault(b => b.Name == brandname && b.Country == country);
                CarModel existingModel = _context.CarModels.FirstOrDefault(m => m.Name == modelname && m.Year == modelyearint && m.Brand == existingBrand);
                CarEngine existingEngine = _context.CarEngines.FirstOrDefault(e => e.Name == enginename && e.Capacity == enginecapacityfloat);

                if (existingBrand != null && existingModel != null && existingEngine != null)
                {
                    Car newCar = new Car() { Model = existingModel, Engine = existingEngine };
                    _context.Cars.Add(newCar);
                    _context.SaveChanges();
                }
                else TempData["Error"] = "Внимание! Данная машина уже существует либо вы ввели несуществующие марку, модель или двигатель, необходимо проверить.";
            }
            else
            {
                TempData["Error"] = "Ошибка при создании. Возможно, вы не заполнили нужные поля. Прочитайте инструкцию ниже.";
                return Redirect("../moderator");
            }
            return Redirect("../moderator");
        }
        [Route("/delete")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(int? carid, int? engineid, int? modelid, int? brandid)
        {
            try
            {
                if (carid != null)
                {
                    var car = await _context.Cars.FindAsync(carid);
                    if (car != null)
                    {
                        _context.Cars.Remove(car);
                    }
                }

                if (engineid != null)
                {
                    var engine = await _context.CarEngines.FindAsync(engineid);
                    if (engine != null)
                    {
                        _context.CarEngines.Remove(engine);
                    }
                }

                if (brandid != null)
                {
                    var brand = await _context.CarBrands.FindAsync(brandid);
                    if (brand != null)
                    {
                        _context.CarBrands.Remove(brand);
                    }
                }

                if (modelid != null)
                {
                    var carmodel = await _context.CarModels.FindAsync(modelid);
                    if (carmodel != null)
                    {
                        _context.CarModels.Remove(carmodel);
                    }
                }
                await _context.SaveChangesAsync();
                return Redirect("../moderator");
            }
            catch (Exception ex) 
            {
                TempData["Error"] = "Ошибка при удалении. Данная машина/марка/модель/двигатель используются пользоваетлями или про них уже написали пост.";
            }
            return Redirect("../moderator");

        }
    }
}
