$(() => {
    $(".like").hide();
    $(".dislike").hide();
    $(".likeft").on('click', () => {
        const id = $(".likeft").data('id');        
        $.post('/home/likejoke', { id }, () => {         
            $(".likeft").hide();
            $(".dislike").show();            
        });
        setTimeout(disableboth, 100000)
    });
    $(".dislike").on('click', () => {
        const id = $(".dislike").data('id');
        const like = false;
        $.post('/home/updatejoke', { id, like }, () => {
            $(".like").show();
            $(".dislike").hide();
        });
    });
    $(".like").on('click', () => {
        const id = $(".like").data('id');
        const like = true;
        $.post('/home/updatejoke', { id, like }, () => {
            $(".like").hide();
            $(".dislike").show();
        });
    });
    function disableboth() {
        $(".like").hide();
        $(".dislike").hide();
    }
  
});

//function myFunction() {
//    var x = document.getElementById("myDIV");
//    if (x.style.display === "none") {
//        x.style.display = "block";
//    } else {
//        x.style.display = "none";
//    }
//}