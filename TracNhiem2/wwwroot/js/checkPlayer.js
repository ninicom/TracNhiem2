function isExitPlayer(user_id) {
    $.ajax({
        url: '/Player/PlayerExists',
        type: 'get',
        datatype: 'json',
        data: {
            Id: user_id,
        },
        success: function (data) {
            
            if (data.playerExists) {
                localStorage.setItem('user_id', data.player.id);
                localStorage.setItem('user_name', data.player.name);
                return true;
            }
            else {
                window.location.href = '../Player'
                return false;
            }
        }
    });
};
function checkPlayer() {
    var user_id = localStorage.getItem('user_id');
    isExitPlayer(user_id);
};
window.onload = checkPlayer;