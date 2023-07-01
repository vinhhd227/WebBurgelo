// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// function openList(listName) {
//     var i;
//     var x = document.getElementsByClassName("list");
//     for (i = 0; i < x.length; i++) {
//       x[i].style.display = "none";
//     }
//     document.getElementById(listName).style.display = "block";
//   }
  
function loadslick(){
    $('.responsive').slick({
      dots: true,
      infinite: true,
      speed: 300,
      slidesToShow: 4,
      slidesToScroll: 1,
      responsive: [
        {
          breakpoint: 1085,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1
          }
        },
        {
          breakpoint: 768,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1
          }
        }
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ]
    });
  }
  loadslick();
  

  function showFood(element){
    let collection = document.getElementsByClassName("food_menu_list");
    $('.responsive').slick('unslick');
    console.log(collection);
    for(let i of collection){
      console.log(i);
      i.classList.add("d-none");
    }
    let key = element.id.slice(10,).toString();
    console.log(key);
    let getID = document.getElementById(key.toString());
    getID.classList.remove("d-none");
    getID.classList.remove("d-none");

    loadslick();
  }