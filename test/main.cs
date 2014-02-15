exec("tasman/main.cs");
Tasman.globals(true);
exec("./tests.spec.cs");

function onStart() {
   GlobalActionMap.bind("keyboard", "escape", "quit");
}

function onEnd() {
}

function GameConnection::onEnterGame() {
   toggleConsole(true);
   Tasman.runAll();
   Tasman.cleanUp();
}
