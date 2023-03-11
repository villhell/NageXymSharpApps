let start_sound = new Audio("/audio/ding.ogg");
let end_sound = new Audio("/audio/ding2.ogg");

export function start() {
    start_sound.play();
}

export function end() {
    end_sound.play();
}