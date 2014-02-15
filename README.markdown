# Tasman

A TorqueScript testing framework for [T3D][] inspired by [Jasmine][].

 [T3D]: https://github.com/GarageGames/Torque3D
 [Jasmine]: http://pivotal.github.io/jasmine/

# Example


    test(MyObject);

    function MyObjectTests::before() {
       new ScriptObject(MyObject) {
          property = "value";
          point = "20.5 19 -3";
       };
    }

    function MyObjectTests::after() {
       MyObject.delete();
    }

    function MyObjectShould::exist() {
       expect(MyObject).toBeAnObject();
    }

    function MyObjectShould::have_a_property() {
       expect(MyObject.property).toEqual("value");
    }
