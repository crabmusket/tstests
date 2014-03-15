exec("tasman/main.cs");
Tasman::create();

// Load tests for specific areas.
exec("./types.cs");

function onStart() {
   GlobalActionMap.bind("keyboard", "escape", "quit");
   GlobalActionMap.bind("keyboard", "alt f4", "quit");
}

function onEnd() {
   Tasman::destroy();
}

function GameConnection::onEnterGame() {
   toggleConsole(true);
   Tasman.runAll();
}
