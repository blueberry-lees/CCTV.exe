// === ROUND 1 START ===
 ->chapter_0

// tags contain 
//#speaker: Male, Female, Narrator
//#SFX:
//#expression:
//#character: Killer, You
//#effect

//switch cases

{progress:
- 0: ->chapter_0
- 1: ->chapter_1
- 2: ->chapter_2
- 3: ->chapter_3
- 4: ->chapter_4
- 5: ->chapter_5

}

/* ---------------------------------

   Functions

 ----------------------------------*/
CONST INITIAL_SWING = 1001

=== function swing_count(x) 
    ~ return (upness(x) + downness(x)) - 2
=== function swing_ready(x) 
    ~ return swing_count(x) >= 2
=== function raise(ref x)
	~ x = x + 1000
=== function elevate(ref x)
    ~ raise(x)
    ~ raise(x)
    ~ raise(x)
=== function lower(ref x)
	~ x = x + 1 
== function ditch (ref x) 
    ~ lower(x) 
    ~ lower(x) 
    ~ lower(x)
=== function demolish(ref x)
    ~ x = x + 20
=== function escalate(ref x)
    ~ x = x + (20 * 1000)
=== function upness(x)
	~ return x / 1000

=== function downness(x)
	~ return x % 1000

=== function high(x)
    ~ return (1 * upness(x) >= downness(x) * 9)

=== function up(x)
	~ return swing_ready(x) && (4 * upness(x) >= downness(x) * 6)

=== function down(x)
	~ return swing_ready(x) && (6 * upness(x) <= downness(x) * 4)
	
=== function low(x)
	~ return swing_ready(x) && (9 * upness(x) <= downness(x) * 1)
	
=== function mid(x)
    // If the swing isn't ready this returns true 
    // Because "up is false and down is false"
    ~ return (not up(x) && not down(x))


/* ---------------------------------

   Global Variables

 ----------------------------------*/

VAR progress = 0

VAR trust = INITIAL_SWING
VAR delusion = INITIAL_SWING

//var for choices

VAR stabbed = false
VAR refuseToStab = false


LIST ending = (none), A, B, C, D


//
// Content
//

 === chapter_0

//elevator_intro 
~progress = 0
#SFX: Ding
#background: ElevatorOpen
#speaker:Narrator
The elevator dings.
#pause(1.0)

You step into the elevator, the soft hum of machinery buzzing faintly beneath your feet. 
#pause(1.0)

It's cold. Sterile. Like the air is waiting for something to happen.

#pause(1.0)

The doors begin to close. But just before they shut—

#Background: HandOnDoor
A hand stops them.

#pause(0.5)
#walk sound
#background man foot walk in
And a man steps in.

 ->chapter_1
 === chapter_1
 ~progress = 1
~ temp confidence = 0
//describe_man 


#character: Killer
#expression: NeutralLookAway
#background look man down to shoulder and stop
The man's average. Tallish. Pale. Clean suit. Polished shoes.

But there’s something... off.


*   [look away] You keep your gaze on the floor. The fluorescent lights reflect in the polished steel.
    ~ confidence = 0

    
*   [stare at him] You meet his eyes. His eyes don’t quite match his smile. 
    Something sharp glints in them. Not metal. Something worse.
    ~ confidence = 1

  
-



 ->chapter_2
 === chapter_2
 ~progress = 2
~ temp confidence = 0

//small_talk_start 


#speaker: Narrator
The elevator begins its slow descent.
The silence stretches.
Too long.
He finally speaks.

#speaker: Male
#character: Killer
#expression:Smile1
"Going down?"

#speaker: Narrator
You didn’t answer right away. Something in your throat locks.
 #speaker:Female

*   {confidence <= 0}[nervous smile] "Y-yeah. Just... just heading down."
    ~ confidence = 0 

*   {confidence >= 0}[bluntly]  "Yes."
    ~ confidence = 1
    
*   {confidence > 0}[make a joke]  "Only way to go from the top, right?"
    ~ confidence = 2
 


-
#speaker: Male
#character: Killer
#expression:SmileLookAway 
"Suppose so."  
"Funny thing, these little boxes."
"Tiny worlds. No escape. "
"Just you and... whoever walks in."

#speaker:Narrator
#character: Killer
#expression:NeutralLookAway 
He pauses. Like he’s letting the words hang in the air for too long.
...
#expression:NeutralLook 
Like he's watching them sink into your skin.
#speaker:Female

*   {confidence == 2 or confidence == 0}[awkward laugh]  "Yeah... elevators are weird."

    
*   {confidence <= 1}[tense]  "What do you mean by that?"
    ~lower(trust)
 
    
*   {confidence >= 1}[joking]  "Sounds like a setup for a horror movie."
    ~ raise(trust)
      
    
-
#speaker: Male
#character: Killer
#expression:SmileLook
"Horror... hmm. Maybe."
"Or just a moment where everything slips."
"When you realize you were never alone to begin with."

#speaker:Narrator
You laugh nervously, but it dies in your throat.


 ->chapter_3
 === chapter_3
//delusion_hint 
~progress = 3

#speaker:Narrator

There’s something wrong.

His reflection in the elevator mirror behind him—it doesn’t move when he does.

You blink. 

Look again.

It’s back to normal.

...Was it?

*   [rub your eyes] You blink a few more times, heart thudding. Maybe you’re tired.
 ~ lower(delusion)
    
*   [look again]  You focus on the reflection. It stares back. Eyes too wide.
~ raise(delusion)

*   [step back slightly]  You shift away, gripping your bag tighter.

-

//checking variables
(Trust - Up: {upness(trust)}, Down: {downness(trust)})
(Delulu - Up: {upness(delusion)}, Down: {downness(delusion)})


 ->chapter_4
 === chapter_4
 ~progress = 4

//voice_in_head 


#show_expression("paranoid")
#speaker: Narrator
A whisper curls in your mind. Soft. Familiar.

"He’s not real. He’s not. Not this time."

You shake your head. The whisper is gone.

The man’s still smiling. But his eyes... they flicker for just a moment.  
Black, like a void.

Then they’re normal again.

#speaker: Male
#character: Killer
#expression:NeutralLook  
"You alright? You look pale."

#speaker: Female

*   [lie]  "Just tired. Didn’t sleep well."
    ~ lower(trust)
*   [avoid]  "I’m fine."
    ~mid(trust)
*   [truth]  "Do you ever feel like someone’s lying to you? Even if they say all the right things?"
    ~ raise(trust)
-
#speaker: Male
#character: Killer
#expression:SmileLook  

#pause(1.0)
"All the time."

//checking variables
(Trust - Up: {upness(trust)}, Down: {downness(trust)})
(Delulu - Up: {upness(delusion)}, Down: {downness(delusion)})


 ->chapter_5
 === chapter_5
 
~progress = 5
#speaker: Narrator
The elevator dings.

You’re almost at the lobby.

He steps closer. 
Just slightly. Almost imperceptibly.

But you notice.

#speaker: Male
#character: Killer
#expression:Smile1
"I think we'll be seeing more of each other."

#speaker: Female

*   {down(trust)} [freeze]  You don't respond. Can't.

*   {down(trust) or mid(trust)}[respond]  "I... hope not."

*   {mid(trust) or up(trust)}[fake a smile] "Looking forward to it."
    
-
#speaker: Narrator
The elevator doors open.

He steps out, but just before turning the corner, he glances back.

And he *winks*.

#show_expression("disturbed")
#pause(1.0)

You don’t remember pressing the emergency stop, but the elevator halts again.

You’re alone.

And yet…  
you still feel watched.


#add a cctv angle here

-> END

//checking variables
(Trust - Up: {upness(trust)}, Down: {downness(trust)})
(Delulu - Up: {upness(delusion)}, Down: {downness(delusion)})
