using DrugShoppingCartMvcUI.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DrugShoppingCartMvcUI.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class DrugController : Controller
{
    private readonly IDrugRepository _drugRepo;
    private readonly IGenreRepository _genreRepo;
    private readonly IFileService _fileService;

    public DrugController(IDrugRepository drugRepo, IGenreRepository genreRepo, IFileService fileService)
    {
        _drugRepo = drugRepo;
        _genreRepo = genreRepo;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var drugs = await _drugRepo.GetDrugs();
        return View(drugs);
    }

    public async Task<IActionResult> AddDrug()
    {
        var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem
        {
            Text = genre.CategoryName,
            Value = genre.Id.ToString(),
        });
        DrugDTO drugToAdd = new() { GenreList = genreSelectList };
        return View(drugToAdd);
    }

    [HttpPost]
    public async Task<IActionResult> AddDrug(DrugDTO drugToAdd)
    {
        var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem
        {
            Text = genre.CategoryName,
            Value = genre.Id.ToString(),
        });
        drugToAdd.GenreList = genreSelectList;

        if (!ModelState.IsValid)
            return View(drugToAdd);

        try
        {
            if (drugToAdd.ImageFile != null)
            {
                if(drugToAdd.ImageFile.Length> 1 * 2048 * 2048)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg",".jpg",".png"];
                string imageName=await _fileService.SaveFile(drugToAdd.ImageFile, allowedExtensions);
                drugToAdd.Image = imageName;
            }
            // manual mapping of DrugDTO -> Drug this is 
            Drug drug = new()
            {
                Id = drugToAdd.Id,
                DrugName = drugToAdd.DrugName,
                Producer = drugToAdd.Producer,
                Image = drugToAdd.Image,
                GenreId = drugToAdd.GenreId,
                Price = drugToAdd.Price
            };
            await _drugRepo.AddDrug(drug);
            TempData["successMessage"] = "Drug is added successfully";
            return RedirectToAction(nameof(AddDrug));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"]= ex.Message;
            return View(drugToAdd);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(drugToAdd);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(drugToAdd);
        }
    }

    public async Task<IActionResult> UpdateDrug(int id)
    {
        var drug = await _drugRepo.GetDrugById(id);
        if(drug==null)
        {
            TempData["errorMessage"] = $"Drug with the id: {id} does not found";
            return RedirectToAction(nameof(Index));
        }
        var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem
        {
            Text = genre.CategoryName,
            Value = genre.Id.ToString(),
            Selected=genre.Id==drug.GenreId
        });
        DrugDTO drugToUpdate = new() 
        { 
            GenreList = genreSelectList,
            DrugName=drug.DrugName,
            Producer=drug.Producer,
            GenreId=drug.GenreId,
            Price=drug.Price,
            Image=drug.Image 
        };
        return View(drugToUpdate);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateDrug(DrugDTO drugToUpdate)
    {
        var genreSelectList = (await _genreRepo.GetGenres()).Select(genre => new SelectListItem
        {
            Text = genre.CategoryName,
            Value = genre.Id.ToString(),
            Selected=genre.Id==drugToUpdate.GenreId
        });
        drugToUpdate.GenreList = genreSelectList;

        if (!ModelState.IsValid)
            return View(drugToUpdate);

        try
        {
            string oldImage = "";
            if (drugToUpdate.ImageFile != null)
            {
                if (drugToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg", ".jpg", ".png"];
                string imageName = await _fileService.SaveFile(drugToUpdate.ImageFile, allowedExtensions);
                // hold the old image name. Because we will delete this image after updating the new
                oldImage = drugToUpdate.Image;
                drugToUpdate.Image = imageName;
            }
            // manual mapping of DrugDTO -> Drug
            Drug drug = new()
            {
                Id=drugToUpdate.Id,
                DrugName = drugToUpdate.DrugName,
                Producer = drugToUpdate.Producer,
                GenreId = drugToUpdate.GenreId,
                Price = drugToUpdate.Price,
                Image = drugToUpdate.Image
            };
            await _drugRepo.UpdateDrug(drug);
            // if image is updated, then delete it from the folder too
            if(!string.IsNullOrWhiteSpace(oldImage))
            {
                _fileService.DeleteFile(oldImage);
            }
            TempData["successMessage"] = "Drug is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(drugToUpdate);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(drugToUpdate);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(drugToUpdate);
        }
    }

    public async Task<IActionResult> DeleteDrug(int id)
    {
        try
        {
            var drug = await _drugRepo.GetDrugById(id);
            if (drug == null)
            {
                TempData["errorMessage"] = $"Drug with the id: {id} does not found";
            }
            else
            {
                await _drugRepo.DeleteDrug(drug);
                if (!string.IsNullOrWhiteSpace(drug.Image))
                {
                    _fileService.DeleteFile(drug.Image);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on deleting the data";
        }
        return RedirectToAction(nameof(Index));
    }

}
