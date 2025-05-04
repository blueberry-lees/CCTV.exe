VAR inThought = false
VAR speaker = ""

// tags contain 
//#speaker:
//#SFX:
//#expression:
//#effect



-> round_1
=== round_1 ===
 #SFX: Ding
 #speaker:Narrator
(The elevator dings.)

#background: Elevator
(The doors slide open. Inside is a man — overweight, sweating, breathing just a bit too loudly. He looks up.)



 #speaker:Female
"Hi."

#speaker:Narrator
(She greeted the man.)

#speaker:Female
  "..."

 #speaker:Narrator
(He barely responds. Just a nod.)

*[walk in] -> stuck_elevator

=== stuck_elevator ===

#SFX: High_heel_walking
#speaker:Female
 #expression:InnerThought
( I step inside.)

#SFX: Elavator_door_close
#speaker:Female
 #expression:InnerThought
(The doors close behind me with a heavy thud.)

#SFX: Click
#SFX: Screech
#speaker:Female
 #expression:InnerThought
(I hear a sudden *click*—the familiar hum of movement... doesn't come.)


#speaker:Female
 #expression:InnerThought
 (???)
 
 #speaker:Female
 #expression:InnerThought
 (I walk back to check the elevator panel.)
*[Presses the 'Open' button.] -> open_button_first

*[Tries the 'Emergency' button.] -> emergency_button_first





= open_button_first
#SFX: Elevator_button
 #speaker:Narrator
(Nothing.)
*[Tries the 'Emergency' button.]  ->emergency_button

= emergency_button
#SFX: Elevator_button
 #speaker:Narrator
(...no response.) 
-> stuck_elevator_continued




= emergency_button_first
#SFX: Elevator_button
 #speaker:Narrator
(...no response.)
*[Presses the 'Open' button.] -> open_button

= open_button
#SFX: Elevator_button
 #speaker:Narrator
(Nothing.) 
-> stuck_elevator_continued





= stuck_elevator_continued

 #speaker:Female 
#expression:Neutral
"What the...?"

#speaker:Female
 #expression:InnerThought
(Why isn't it working?)

#speaker:Male 
#expression:Neutral
"I think... we might be stuck." *quietly*

#speaker:Female
 #expression:InnerThought
(No. No, no, no...)

#speaker:Female  
#expression:Neutral
"You mean the elevator stopped?"

#speaker:Male 
#expression:Neutral
"Looks like it."

#speaker:Female 
#expression:Neutral
"..."

*   [Panic.] -> panic_mode
*   [Stay calm.] -> keep_composure





=== panic_mode ===
#speaker:Female
#expression:Neutral
"This can't be happening!"

*[pound on the door]

#SFX:Pound
#speaker:Female
 #expression:InnerThought
(I pound on the door. No sound from outside.)

...


#speaker:Male
#expression:Neutral
"Hey... It's probably just a short. These things reset."


#speaker:Female
#expression:Neutral
"You don’t know that..."

-> silence_start





=== keep_composure ===
#speaker:Female
#expression:Neutral
"Okay. Okay. Maybe it’s temporary."


#speaker:Male
#expression:Neutral
"Probably. These things reboot."

#speaker:Female
#expression:Neutral
"Hopefully soon..."


-> silence_start






=== silence_start ===
#speaker:Male
#expression:Neutral
"..."

* [Stay silent.]
#speaker:Female
#expression:Neutral

"..."

#speaker:Narrator
#expression:Default
(The silence stretches...)


#speaker:Male
#expression:Neutral
"..."

->still_silent



=== still_silent ===

* [Still stay silent.]
#speaker:Female
#expression:Neutral
"..."


-> interaction_start

=== interaction_start ===
 #speaker:Male
#expression:Neutral
(He clears his throat.)


#speaker:Male
"You work on this floor?" 
* ["Yeah"] -> smalltalk_yes
* ["Don’t talk to me."] -> smalltalk_no




=== smalltalk_yes ===
#speaker:Female
#expression:Neutral
"Yeah. Just started last week."


#speaker:Male
#expression:Neutral
"Oh. Welcome to hell, then." *chuckles*

#speaker:Female
#expression:Neutral
"..."


#speaker:Female
 #expression:InnerThought
(Was that supposed to be funny?)

-> unease_grows



=== smalltalk_no ===
#speaker:Female
#expression:Neutral
"I’d prefer not to chat."


#speaker:Male
#expression:Neutral
"...Alright then."


#speaker:Female
#expression:Neutral
"..."

-> unease_grows




=== unease_grows ===
~ inThought = true
#speaker:Female
 #expression:InnerThought
(The air feels thick.)

#speaker:Female
 #expression:InnerThought
(His eyes flick toward me now and then... Or am I imagining that?)


#speaker:Female
#expression:Neutral
"So… you think they’ll fix this soon?"


#speaker:Male
#expression:Neutral
"Shouldn’t take long."


#speaker:Female
 #expression:InnerThought
(He’s closer now. )

#speaker:Female
 #expression:InnerThought
(Just slightly. )
#speaker:Female
 #expression:InnerThought
(Maybe I shifted back, maybe he forward... )


#speaker:Male
#expression:Neutral
"Ha..."


#speaker:Female
 #expression:InnerThought
 (Yea. He's definitely getting closer...)

-> escalation_choice





=== escalation_choice ===
#speaker:Female
 #expression:InnerThought
*    ["Why are you sweating so much?"]
    -> awkward_tension
    //(She imagines him reaching toward her.)
*    ["Don’t touch me!"]
    -> accusation_made




=== awkward_tension ===
#speaker:Female
#expression:Neutral
"You okay? You’re... sweating a lot."


#speaker:Male
#expression:Neutral
"Yeah, just—heat. Hate small spaces."


#speaker:Female
#expression:Neutral
"Same..."

~ inThought = true
*[Something’s off.] -> accusation_made
*[Something’s off.] -> accusation_made
*[Something’s off.]-> accusation_made





=== accusation_made ===
#speaker:Female
#expression:Neutral
"Don’t touch me!"


#speaker:Male
#expression:Neutral
"What?! I didn’t—"


#speaker:Female
#expression:Neutral
"Back off!"


#speaker:Female
 #expression:InnerThought
(I can’t breathe.)
#speaker:Female
 #expression:InnerThought
(I reach into my bag—)
//play sfx

-> attack_decision




=== attack_decision ===
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1





=== attack_ending_1 ===

#speaker:Female 
#expression:Neutral
"Stay away from me!"


#speaker:Male
#expression:Neutral
"Wait! Please—"

 #speaker:Narrator
(She lunges.)
#speaker:Narrator
(The elevator doors open. Chaos. Voices. Police.)


#speaker:Female
#expression:Neutral
"He—he was going to—!"


#speaker:Narrator
(The screen flickers. CCTV footage plays: grainy, overhead. The girl stands tall. The man is hunched, small.)
#speaker:Narrator
(There's no motion from him — until the moment she attacks...)

*[return to preview] -> return_to_preview_1




=== return_to_preview_1 ===
*[Launch review] ->replay

=replay
[FILE ALREADY ACCESSED. REPLAY?]

*   [Yes.] -> replay_round_2
*   [No.]  -> return_to_preview_1




=== replay_round_2 ===
[ROUND 2]
// Next segment to be scripted
->DONE
