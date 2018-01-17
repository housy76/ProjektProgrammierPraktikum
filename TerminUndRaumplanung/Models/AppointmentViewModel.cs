using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TerminUndRaumplanung.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AppointmentViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int AppointmentId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Beginn")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Ende")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Ort")]
        public Room Room { get; set; }


        /// <summary>
        /// 
        /// </summary>
        //necessary for the dropdownlists to store the selected
        //value and send it back to the controller.
        [Required(ErrorMessage = "Ort ist erforderlich!")]
        public int SelectedRoom { get; set; }


        
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Weitere Ressourcen")]
        public List<Ressource> Ressources { get; set; }


        /// <summary>
        /// entity for many-to-many connection
        /// </summary>
        public List<AppointmentRessource> AppointmentRessources { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public IEnumerable<int> SelectedRessource { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Survey Survey { get; set; }
    }
}