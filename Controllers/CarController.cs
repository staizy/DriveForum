using DriveForum.DatabaseContext;
using DriveForum.Models;
using DriveForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View(carsForModerator);
        }

        [Route("moderator/add")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> NewCar(string brandname, string country, string modelname, string modelyear, string enginename, string enginecapacity)
        {
            if (brandname != null && country != null && modelname == null && modelyear == null && enginename == null && enginecapacity == null)
            {
                if ((await _context.CarBrands.FirstOrDefaultAsync(b => b.Name == brandname && b.Country == country)) == null)
                {
                    CarBrand newCarBrand = new CarBrand() { Name = brandname, Country = country, CarModels = new() };
                    await _context.CarBrands.AddAsync(newCarBrand);
                    await _context.SaveChangesAsync();
                }
                else TempData["Error"] = "Внимание! Данная марка уже существует.";
            }
            else if (brandname == null && country == null && modelname == null && modelyear == null && enginename != null && enginecapacity != null)
            {
                float enginecapacityfloat = float.Parse(enginecapacity.Replace('.', ','));
                if ((await _context.CarEngines.FirstOrDefaultAsync(e => e.Name == enginename && e.Capacity == enginecapacityfloat) == null))
                {
                    CarEngine newCarEngine = new CarEngine() { Name = enginename, Capacity = enginecapacityfloat };
                    await _context.CarEngines.AddAsync(newCarEngine);
                    await _context.SaveChangesAsync();
                }
                else TempData["Error"] = "Внимание! Данный двигатель уже существует.";
            }

            else if (brandname != null && country != null && modelname != null && modelyear != null && enginename == null && enginecapacity == null)
            {
                int modelyearint = int.Parse(modelyear);
                var existingBrand = await _context.CarBrands.Include(u => u.CarModels).FirstOrDefaultAsync(b => b.Name == brandname && b.Country == country);
                if (existingBrand != null)
                {
                    var existingModel = existingBrand?.CarModels?.FirstOrDefault(m => m.Name == modelname && m.Year == modelyearint);
                    if (existingModel == null)
                    {
                        CarModel newCarModel = new CarModel() { Name = modelname, Year = modelyearint, Brand = existingBrand };
                        await _context.CarModels.AddAsync(newCarModel);
                        await _context.SaveChangesAsync();
                    }
                    else TempData["Error"] = "Внимание! Данная модель у марки уже существует.";
                }
                else TempData["Error"] = "Внимание! Данной марки не существует, необходимо добавить.";
            }

            else if (brandname != null && country != null && modelname != null && modelyear != null && enginename != null && enginecapacity != null)
            {
                float enginecapacityfloat = float.Parse(enginecapacity.Replace('.', ','));
                int modelyearint = int.Parse(modelyear);
                CarBrand existingBrand = await _context.CarBrands.Include(u => u.CarModels).FirstOrDefaultAsync(b => b.Name == brandname && b.Country == country);
                CarModel existingModel = await _context.CarModels.FirstOrDefaultAsync(m => m.Name == modelname && m.Year == modelyearint && m.Brand == existingBrand);
                CarEngine existingEngine = await _context.CarEngines.FirstOrDefaultAsync(e => e.Name == enginename && e.Capacity == enginecapacityfloat);

                if (existingBrand != null && existingModel != null && existingEngine != null)
                {
                    Car newCar = new Car() { Model = existingModel, Engine = existingEngine };
                    await _context.Cars.AddAsync(newCar);
                    await _context.SaveChangesAsync();
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
