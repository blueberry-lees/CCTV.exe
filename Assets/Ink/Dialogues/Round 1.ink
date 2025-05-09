// tags contain 

//#speaker:

//#SFX:

//#expression:

//#effect



-> Round1_start

=== Round1_start ===

#SFX: Ding
#background: ElevatorDark
#speaker:Narrator
The elevator dings.

#background: ElevatorOpen
The doors slide open. Inside is a man — nervous, sweating, breathing just a bit too loudly. 

He looks up.


#speaker:Female
#expression:InnerThought
"Hi."

#speaker:Narrator
She greeted the man.


#speaker:Male
#expression:Shadow
"..."

#speaker:Narrator
He barely responds. Just a nod.

->enter_elevator
= enter_elevator

+[stay at the entrance] 
    You waited outside, but noyhong happens. 
    ->enter_elevator
    +[walk in] -> Stuck_elevator


= Stuck_elevator

#speaker:Female
#expression:InnerThought
...

#SFX: High_heel_walking
#speaker:Narrator
You step inside.

#SFX: Elavator_door_close
#background: Elevator
#speaker:Narrator
The doors close behind you with a heavy thud.

#SFX: Click
#SFX: Screech
#background: ElevatorDark
#speaker:Narrator
You hear a sudden *click*—the familiar hum of movement... doesn't come.

#background: Elevator
#speaker:Female
#expression:InnerThought
(???)


#speaker:Narrator
You walk back to check the elevator panel.

*[Presses the 'Open' button.] #SFX: Elevator_button 
    Nothing. #speaker:Narrator
    
    **[Tries the 'Emergency' button.] #SFX: Elevator_button
    ...no response.#speaker:Narrator

*[Tries the 'Emergency' button.] #SFX: Elevator_button
    ...no response.  #speaker:Narrator
    
    **[Presses the 'Open' button.] 
    #SFX: Elevator_button
    #speaker:Narrator
    Nothing.
- /////////////

#speaker:Female 
#expression:Neutral
"What the...?"

#speaker:Female
#expression:LightShadow
(Why isn't it working?)


#speaker:Male
#expression:Shadow
"I think... we might be stuck." 

#speaker: Narrator
He mummbled. Quietly. 

#speaker:Female
#expression:InnerThought
(No. No, no, no...)

#speaker:Female  
#expression:Neutral
"You mean the elevator stopped?"

#speaker:Male
#expression:Shadow
"Looks like it."

#speaker:Female 
#expression:Neutral
"..."

*   [Panic.]
    #speaker:Female
    #expression:Neutral
    "This can't be happening!"
    
    **[pound on the door]
    
    #SFX:Pound
    #speaker:Narrator
    You pound on the door. No sound from outside.
    
    ...
    
    #speaker:Male
    #expression:Shadow
    "Hey... It's probably just a short. These things reset."
    
    #speaker:Female
    #expression:Neutral
    "You don’t know that..."

*   [Stay calm.]
    #speaker:Female

    #expression:Neutral
    
    "Okay. Okay. Maybe it’s temporary."
    
    #speaker:Male
    #expression:Shadow
    
    "Probably. These things reboot."
    
    #speaker:Female
    
    #expression:Neutral
    
    "Hopefully soon..."

- //////////
#speaker:Male
#expression:Shadow
"..."

* [Stay silent.]

#speaker:Female
#expression:Neutral
"..."

#speaker:Narrator
The silence stretches...

#speaker:Male
#expression:Shadow
"..."

    ** [Still stay silent.]
    
    #speaker:Female
    #expression:Neutral
    "..."

-> Interaction_start




=== Interaction_start ===

#speaker:Male
#expression:Shadow
"Ehem..."

#speaker: Narrator
He clears his throat.

#speaker:Male
#expression:Shadow

"You work on this floor?" 

* ["Yeah"] 
    #speaker:Female
    
    #expression:Neutral
    
    "Yeah. Just started last week."
    
    #speaker:Male
    #expression:Shadow
    
    "Oh. Welcome to hell, then." *chuckles*
    
    #speaker:Female
    
    #expression:Neutral
    
    "..."
    
    #speaker:Female
    #expression:LightShadow
    
    (Was that supposed to be funny?)

* ["Don’t talk to me."] 
    #speaker:Female
    
    #expression:Neutral
    
    "I’d prefer not to chat."
    
    #speaker:Male
    #expression:Shadow
    
    "...Alright then."
    
    #speaker:Female
    
    #expression:Neutral
    
    "..."

- //////


-> Unease_grows

= Unease_grows

#speaker:Narrator
The air feels thick.

#speaker:Female
#expression:LightShadow
(His eyes flick toward me now and then... Or am I imagining that?)

#speaker:Female
#expression:Neutral
"So… you think they’ll fix this soon?"

#speaker:Male
#expression:Shadow
"Shouldn’t take long."

#speaker:Female
#expression:LightShadow
(He’s closer now. )
(Just slightly. )


#speaker:Male
#expression:Shadow
"Ha..."

#speaker:Female
#expression:InnerThought
(...Why is he getting closer?)

*[Something’s off.] 
*[Something’s off.] 
*[Something’s off.] 

-
->Tension_rise


= Tension_rise

*    ["Why are you sweating so much?"]

    #speaker:Female
    #expression:Neutral
    "You okay? You’re... sweating a lot."
    
    #speaker:Male
    #expression:Shadow
    "Yeah, just—heat. Hate small spaces."
    
    #speaker:Female
    #expression:Neutral
    "Same..."
    
    #speaker:Male
    #expression:Shadow
    "..."
    
    #speaker:Female
    #expression:Neutral
    "..."
    
    #speaker:Female
    #expression:InnerThought
    (He's moving towards...!)
    
    **[Something’s off.]
    **[Something’s off.]
    **[Something’s off.]
    -- 
*    ["Don’t touch me!"]

-

# speaker:Female, expression:Neutral
"Don’t touch me!"

# speaker:Male, expression:Shadow
"???What?! I didn’t—"

#speaker:Female
#expression:Neutral
"Back off!!!!"

#speaker:Female
#expression:LightShadow
(I can’t breathe.)

#speaker:Female
#expression:LightShadow
(My bag—)

*   [Pull the knife.] 
*   [Pull the knife.] 
*   [Pull the knife.] 
-
-> Attack

= Attack

#speaker:Female 
#expression:Neutral
"Stay away from me!"

#speaker: Narrator
You pulled out the knife from your bag. 

#speaker:Male
#expression:LightShadow
"What!? Wait! Please—"

#speaker:Narrator
You lunged.

#speaker:Narrator
[*Squelch*]

You see red.

The elevator doors open. 
Chaos. 
Voices. 
...police.

They strap you down and hand-cuffed you. 

#speaker:Female
#expression:Neutral
"H—he was going to—!"

#speaker:Narrator
They won't believe you. 
Because...


-> Round1_ending


=== Round1_ending ===
///No narrator part - animation? 
#speaker:Narrator
The screen flickers. CCTV footage plays: grainy, overhead. The girl stands tall. The man is hunched, small.

There's no motion from him — until the moment she attacks...

...

->DONE

