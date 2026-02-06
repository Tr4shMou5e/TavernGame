EXTERNAL RainEvent()
EXTERNAL Relationship()
VAR selectEvent = false

~ selectEvent = true

-> main
=== main ===
I won't deny the way time flies
when we're so close to the end
and now I'm thinking
I spent my life learning to fly
against the constant decent
and now I'm thinking 
    * [This is choice one] // The * is for the choices and the [] is for the option after it gets picked won't show 
        -> Choice_1
    * [This is choice two]
        -> Choice_2
    * [This is choice three]
        -> Choice_3
    
=== Choice_1 ===
You are the chosen one
~ Relationship()
-> END

=== Choice_2 ===
You are not the chosen one!
~ RainEvent()
-> END

=== Choice_3 ===
Welcome to Narnia

{ selectEvent:
    My king is waiting for you
    -else:
        The ice queen will have your head
}
-> END