﻿using AutoMapper;
using Backend.Data.UOW;
using Backend.Dto;
using Backend.Entities;
using Backend.Services.PeopleService;
using Backend.Statics;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.WebApi
{
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IPeopleService service;
        private readonly IMapper _mapper;
        public PeopleController(
            IUnitOfWork uow, 
            IPeopleService service, 
            IMapper _mapper)
        {
            this.uow = uow;
            this.service = service;
            this._mapper = _mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> Create([FromBody]PersonForCreationDto personToCreate)
        {
            if (personToCreate.HasAnyPropertyNullOrEmpty()) return BadRequest("Especifique correctamente la persona a crear");
            var _mappedPerson = _mapper.Map<Person>(personToCreate);
            _mappedPerson = uow.PeopleRepository.Add(_mappedPerson);
            await uow.CompleteAsync();
            return Created($"/people/{_mappedPerson.Id}", _mappedPerson);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var personToDelete = uow.PeopleRepository.GetById(id);
            if(personToDelete is null) return BadRequest("Persona no encontrada");
            uow.CompleteAsync();
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Person>> Update(int id, [FromBody]PersonForModificationDto person)
        {
            var personToModificate = uow.PeopleRepository.GetById(id);
            if(personToModificate is null) return NotFound("Persona no encontrada");
            if(person.HasAnyPropertyNullOrEmpty()) return BadRequest("No debe haber campos vacios o nulos");
            person.Id = id;
            _mapper.Map(person, personToModificate);
            uow.PeopleRepository.Update(personToModificate);
            await uow.CompleteAsync();
            return Ok(personToModificate);
        }
        
        [HttpGet]
        public ActionResult<List<Person>> GetAll()
        {
            return uow.PeopleRepository.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var personRequested = uow.PeopleRepository.GetById(id);
            if(personRequested is null) return NotFound();
            return Ok(personRequested);
        }
    }
}
