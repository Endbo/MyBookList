var results;
$('#myModal').on('hidden.bs.modal', function () {
    var selected_thing = $('input[name=test]:checked').val();
    if (selected_thing != null) {
        $('#Name').val(results[selected_thing]["title"]);
        $('#Year').val(parseInt(results[selected_thing]["publishDate"].substring(0, 4)));
        $('#Synopsis').val(results[selected_thing]["description"]);
        $('#imageUrl').val($('#imageDropx' + selected_thing + ' :selected').val());
    }

});
window.myFunction = function () {
    var name = $("#Name").val();
    if (name) {
        document.getElementById("searchButton").disabled = true;
        $('#search-result').empty();
        $.ajax({
            url: "http://localhost:50297/api/search/" + name,
            context: document.body,
            success: function (data) {
                results = data;
                data.forEach(function callback(currentValue, index, array) {
                    var dropdown = '';
                    for (var key in currentValue.imageLinks) {
                        if (currentValue.imageLinks.hasOwnProperty(key))
                            dropdown += '<option value=' + currentValue.imageLinks[key] + '>' + key + '</option>';
                    }
                    $('#search-result').append(
                        '<div class="modal-body container-fluid" id="search-result>\
                        <div class="row">\
                        <div class="col-md-1">\
                        <input type="radio" name="test" value="'+ index +'">\
                        </div>\
                        <div class="col-md-7">\
                        <p><b>' + currentValue.title +'</b></p>\
                        <p>' + currentValue.publishDate + '</p>\
                        <p>' + currentValue.description.substring(0, 300) +'</p>\
                    </div>\
                    <div class="col-md-4">\
                        <img src="" alt="choose image to see preview"/>\
                     <select name="drop_down" id="imageDropx' + index +'">\
                        '+ dropdown +'</select>\
                    </div>\
                </div>');
                    console.log(currentValue.title);
                });
                $('#myModal').modal('toggle');
                console.log(data);
                document.getElementById("searchButton").disabled = false;
            },
            error: function(data, textStatus, errorThrown) { 
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
                document.getElementById("searchButton").disabled = false;
        }       
        });
    }
}