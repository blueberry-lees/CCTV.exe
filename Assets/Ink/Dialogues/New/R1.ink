
/*
TAGS CONTAIN : 
#speaker:
#SFX:
#expression:
#effect
//#character: Killer, You

*/

//Global Variables

VAR friendly = false
VAR unfriendly = false
VAR neutral = false
VAR trust_level = 0
VAR look_away = false
VAR progress = 0


//Statements
{progress:
- 0: ->chapter_0
- 1: ->chapter_1
- 2: ->chapter_2
- 3: ->chapter_3
- 4: ->chapter_4
- 5: ->chapter_5
}



 ->chapter_0
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

A man steps in.

 ->chapter_1
 === chapter_1
 

//describe_man 
~progress = 1

#character: Killer
#expression: NeutralLookAway
#background: ElevatorDark
He’s average. Tallish. Pale. Clean suit. Polished shoes.

But there’s something... off.

His eyes don’t quite match his smile. One lingers too long. The other darts away too quickly.

You feel it immediately. That gnawing twitch in your chest. The feeling again.

*   [look away] You keep your gaze on the floor. The fluorescent lights reflect in the polished steel.
    ~ look_away = true
    
*   [stare at him] You meet his eyes. Something sharp glints in them. Not metal. Something worse.
    ~ look_away = false

-


 ->chapter_2
 === chapter_2
#background: ElevatorDark
//small_talk_start 
~progress = 2

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
You don’t answer right away. Something in your throat locks.
 #speaker:Female

*   [nervous smile] "Y-yeah. Just... just heading down."

*   [bluntly]  "Yes."

*   [joke]  "Only way to go from the top, right?"
    ~ trust_level = 3

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

#expression:NeutralLook 
Like he's watching them sink into your skin.
#speaker:Female

*   [awkward laugh]  "Yeah... elevators are weird."

*   [tense]  "What do you mean by that?"

*   [joking]  "Sounds like a setup for a horror movie."
    
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

#background: ElevatorDark
//delusion_hint 
~progress = 3

#speaker:Narrator
#character: Killer
#expression:NeutralLookAway
There’s something wrong.

His reflection in the elevator mirror behind him—it doesn’t move when he does.

You blink. 

Look again.

It’s back to normal.

Was it?

*   [rub your eyes] You blink a few more times, heart thudding. Maybe you’re tired.

*   [look again]  You focus on the reflection. It stares back. Eyes too wide.

*   [step back slightly]  You shift away, gripping your bag tighter.
-


 ->chapter_4
 === chapter_4
 #background: ElevatorDark

//voice_in_head 
~progress = 4

#show_expression("paranoid")
#speaker: Narrator
A whisper curls in your mind. Soft. Familiar.

???: "He’s not real. He’s not real. Not this time."

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

*   [avoid]  "I’m fine."

*   [truth]  "Do you ever feel like someone’s lying to you? Even if they say all the right things?"
-
#speaker: Male
#character: Killer
#expression:SmileLook  

#pause(1.0)
"All the time."


 ->chapter_5
 === chapter_5
 #background: ElevatorDark
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

*   [freeze]  You don't respond. Can't.
*   [respond]  "I... hope not."
*   [fake a smile] "Looking forward to it."
    
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

-> END
