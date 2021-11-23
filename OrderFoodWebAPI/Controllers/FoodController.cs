using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataLayer.DTOs;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace OrderFoodWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly IMapper _mapper;

        public FoodController(IFoodService foodService, IMapper mapper)
        {
            _foodService = foodService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFoods()
        {

            try
            {
                var foods = await _foodService.GetAllFoods();
                var result = _mapper.Map<List<FoodDTO>>(foods);
                return Ok(foods);
            }
            catch
            {
                return StatusCode(500, "مشکلی در بارگیری غذا ها پیش آمده است!");
            }
        }
        
        [HttpGet("{id:int}" , Name = "GetFood")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFood(int id)
        {

            try
            {
                var food = await _foodService.GetFoodById(id);
                var result = _mapper.Map<FoodDTO>(food);
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "مشکلی در بارگیری غذا پیش آمده است!");
            }
        }

        [HttpGet("search/{q}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFood(string q)
        {

            try
            {
                var foods = await _foodService.SearchInFoods(q);
                var result = _mapper.Map<List<FoodDTO>>(foods);
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "مشکلی در بارگیری غذا پیش آمده است!");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodDTO foodDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(foodDto);
            try
            {
                var food = _mapper.Map<Food>(foodDto);
                await _foodService.InsertFood(food);

                return CreatedAtRoute("GetFood", new {id = food.Id}, food);
            }
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null)
                return BadRequest();

            string fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(image.FileName)}";
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

            Console.WriteLine(savePath);

            using (Stream stream = new FileStream(savePath,FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return Ok(fileName);
        }

        [HttpDelete("DeleteImage/{imageName}")]
        public async Task<IActionResult> DeleteImage(string imageName)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", imageName);
            System.IO.File.Delete(imagePath);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] UpdateFoodDTO foodDto)
        {
            if (!ModelState.IsValid || id < 1)
                return BadRequest(foodDto);
            try
            {
                var food = await _foodService.GetFoodById(id);
                if (food == null) return NotFound();

                _mapper.Map(foodDto, food);

                await _foodService.UpdateFood(food);

                return NoContent();

            }
            catch (Exception e)
            {
                return Problem(e.Message + "   " + e.InnerException?.Message, statusCode: 500);
            }
        }

        [HttpDelete("{foodId}")]
        public async Task<IActionResult> DeleteFood(int foodId)
        {
            var food = await _foodService.GetFoodById(foodId);
            if (food == null) return NotFound();

            if (!string.IsNullOrEmpty(food.ImageName))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", food.ImageName);
                System.IO.File.Delete(imagePath);
            }

            await _foodService.DeleteFood(food);

            return NoContent();
        }
    }

}
