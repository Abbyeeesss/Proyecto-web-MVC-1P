using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Data;
using Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Controllers
{
    public class NotificacionsController : Controller
    {
        private readonly Proyecto_web_MVC_1PContext _context;

        public NotificacionsController(Proyecto_web_MVC_1PContext context)
        {
            _context = context;
        }

        // GET: Notificacions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notificacion.ToListAsync());
        }

        // GET: Notificacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificacion = await _context.Notificacion
                .FirstOrDefaultAsync(m => m.IdNotificacion == id);
            if (notificacion == null)
            {
                return NotFound();
            }

            return View(notificacion);
        }

        // GET: Notificacions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notificacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNotificacion,IdUsuario,Titulo,Mensaje,FechaEnvio,Estado")] Notificacion notificacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificacion);
        }

        // GET: Notificacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificacion = await _context.Notificacion.FindAsync(id);
            if (notificacion == null)
            {
                return NotFound();
            }
            return View(notificacion);
        }

        // POST: Notificacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNotificacion,IdUsuario,Titulo,Mensaje,FechaEnvio,Estado")] Notificacion notificacion)
        {
            if (id != notificacion.IdNotificacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificacionExists(notificacion.IdNotificacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notificacion);
        }

        // GET: Notificacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificacion = await _context.Notificacion
                .FirstOrDefaultAsync(m => m.IdNotificacion == id);
            if (notificacion == null)
            {
                return NotFound();
            }

            return View(notificacion);
        }

        // POST: Notificacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notificacion = await _context.Notificacion.FindAsync(id);
            if (notificacion != null)
            {
                _context.Notificacion.Remove(notificacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificacionExists(int id)
        {
            return _context.Notificacion.Any(e => e.IdNotificacion == id);
        }
    }
}
