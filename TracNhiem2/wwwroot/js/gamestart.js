
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var currentRoom = window.location.search.split('=')[1];
var user = localStorage.getItem('user_name');
var room = window.location.search.split('=')[1];
var user_id = localStorage.getItem("user_id");

var test = 1;
//Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, score) {

    if (score == 0) {
        alert(`${user} vừa vào phòng`);
        getplayer();
    }
});
connection.on("StartGame", function (start) {
    if (start == true) {
        loadListQuestion();
        var intervalId = setInterval(function () {
            // Kiểm tra cà chờ chơ question
            if (question.length > 0) {
                // Thực hiện hành động khi có dữ liệu mới
                // ...
                info_box.classList.add("activeInfo"); //show info box
                // Xóa bỏ sự kiện định kỳ
                clearInterval(intervalId);
            }
        }, 100);
    }
});

connection.start().then(function () {
    if (user_id == null || user_id == '')
        return;
    joinRoom();
    IsAdmin();
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {

//    var score = parseInt(document.getElementById("messageInput").value);
//    connection.invoke("SendMessage", user, score, room, false).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

$("#start_btn").onclick = () => {
    connection.invoke("GameStart", room, true).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
};
function joinRoom() {
    // code here
    if (room == null || room == '')
        window.location.href = '../Home';
    $("#IdRoom").val(room);
    addplayerToRoom()
    var score = 0;
    console.log(score);
    connection.invoke('OnConnectedAsync', user_id);
    connection.invoke("SendMessage", user, score, room, true).catch(function (err) {
        return console.error(err.toString());
    });
    currentRoom = $("#IdRoom").val();
    event.preventDefault();
};
function IsAdmin() {
    $.ajax({
        url: '../RoomQuiz/IsAdminRoom',
        type: 'Get',
        data: {
            roomId: room,
            playerId: user_id,
        },
        success: function (result) {
            if (result.isAdmin) {
                $("#GameStart").css('display', 'block');
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
function addplayerToRoom() {
    $.ajax({
        url: '../RoomQuizView/addPlayerToRoom',
        type: 'PUT',
        data: {
            roomId: room,
            playerId: user_id,
        },
        success: function (result) {
            getplayer();
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
function getplayer() {
    $.ajax({
        url: '../RoomQuizView/getPlayerFromRoom',
        type: 'GET',
        data: { roomId: room },
        success: function (result) {
            loadLeaderBoard(result.players);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}










function loadLeaderBoard(players) {
    clearLeaderboard();
    $.each(players, function (key, value) {
        AddPlayerToLeaderBoard(value.id, value.name, value.score, key);
    });
}
function clearLeaderboard() {
    var leaderboard = document.querySelector(".leaderboard");
    leaderboard.innerHTML = "";
}

function AddPlayerToLeaderBoard(id, name, score, index) {
    // Tạo phần tử HTML mới
    var div = document.createElement("div");
    div.id = id;
    div.className = "row";

    // Thêm nội dung cho phần tử HTML mới
    var medal = createMedal(index);

    var namePlayer = document.createElement("div");
    namePlayer.className = "name";
    namePlayer.innerHTML = name;

    var scorePlayer = document.createElement("div");
    scorePlayer.className = "score";
    scorePlayer.innerHTML = score;

    div.appendChild(medal);
    div.appendChild(namePlayer);
    div.appendChild(scorePlayer);

    // Thêm phần tử mới vào phần tử HTML đã cho
    var leaderboard = document.querySelector(".leaderboard");
    leaderboard.appendChild(div);
}

function createMedal(index) {
    var medal = document.createElement("div");
    switch (index) {
        case 0:
            medal.className = "gold-medal";
            medal.innerHTML = "🥇";
            break;
        case 1:
            medal.className = "silver-medal";
            medal.innerHTML = "🥈";
            break;
        case 2:
            medal.className = "bronze-medal";
            medal.innerHTML = "🥉";
            break;
        default:
            medal.className = "medal";
            medal.innerHTML = "🏅";
    }
    return medal;
}
