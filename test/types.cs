//-----------------------------------------------------------------------------
// types.cs
// Unit tests for TS's type system, such as it is. This commonly refers to
// interaction between different literal types - for example, what happens when
// you add an integer and a string?
//-----------------------------------------------------------------------------

test(Strings);

function StringsShould::be_comparable_with_strequals() {
   // toBe compares with $=
   expect("").toBe("");
   expect("a").not().toBe("");
   expect("").not().toBe("b");
   expect("c").toBe("c");
   expect("d").not().toBe("e");
}

function StringsShould::be_case_insensitive() {
   expect("hi").toBe("HI");
   expect("ho").toBe("HO");
   expect("dfjdkfjdf").toBe("DFJDKFJDF");
}

function StringsShould::always_compare_true_with_equals() {
   // toEqual compares with ==
   expect("trip").toEqual("trip");
   expect("trap").toEqual("TRAP");
   expect("ping").toEqual("pong");
   expect("pow").toEqual("");
   expect("crash").toEqual("sdkhskhsdf");
}

function StringsShould::be_constructed_without_quotes() {
   expect(yellow).toBe("yellow");
   expect(red).toBe("RED");
   expect().toBe("");
}

function StringsShould::cast_to_zero_when_nonnumeric() {
   expect("luis" + 0).toEqual(0);
   expect("jeff" + 1).toEqual(1);
   expect("bank" - 2).toEqual(-2);
   expect("thomas" * 2).toEqual(0);
   expect("andrew" / 2).toEqual(0);
   expect("daniel" / -1).toBe("-0");
}

function StringsShould::cast_to_numbers_when_numeric() {
   expect("1" + 1).toEqual(2);
   expect("9" - 6).toEqual(3);
   expect("6" * 2).toEqual(12);
   expect("8" / 4).toEqual(2);
   expect("8.9" + 0.1).toEqual(9);
}

//-----------------------------------------------------------------------------

test(Numbers);

function NumbersShould::be_constructed_as_ints_or_floats() {
   // toEqual compares with ==
   expect(0).toEqual(0.0);
   expect(1).toEqual(1.0);
   expect(1000).toEqual(1000.0);
   expect(-2).toEqual(-2.0);
   expect(-0).toEqual(0);
   expect(5).not().toEqual(5.1);
}

function NumbersShould::be_cast_to_strings() {
   // toBe compares with $=
   expect(1000).not().toBe("1000.0");
   expect(1001).toEqual("1001.0");
   expect(0).not().toBe("-0");
   expect(0).toEqual("-0");
   expect(1000.0).toBe("1000");
   expect(0 @ 1).toBe("01");
   expect("goodbye" @ 5).toBe("goodbye5");
}

function NumbersShould::obey_order_of_operations() {
   expect(2 * 2 + 1).toEqual(5);
   expect(2 * (2 + 1)).toEqual(6);
   expect(2 / 2 + 1).toEqual(2);
   expect(2 / (2 + 1)).toEqual(2/3);
   expect(2 * 2 - 1).toEqual(3);
   expect(2 * (2 - 1)).toEqual(2);
   expect(2 / 2 - 1).toEqual(0);
   expect(2 / (2 - 1)).toEqual(2);

   // << and >> have low precedence.
   expect(1 << 1 + 1).toEqual(4); 
   expect((1 << 1) + 1).toEqual(3); 
   expect(4 >> 1 + 1).toEqual(1);
   expect((4 >> 1) + 1).toEqual(3);
}

function NumbersShould::compare() {
   %n = 2/3;
   %p = 2/3;
   expect(%n == 2/3).toHold();
   expect(%n == %p).toHold();
   expect(2/3 == 2/3).toHold();
}

function NumbersShould::shift_() { // logically or arithmetically?
   expect(0 >> 1).toEqual(0);
   expect(0 << 1).toEqual(0);
}
