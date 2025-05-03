VAR inThought = false
VAR speaker = ""

// tags contain 
//#speaker:
//#SFX:
//#expression:
//#effect


{inThought:
    // If inThought is true, don't show the speaker's name
    ~ speaker = "Narrator"
- else:
    // If inThought is false, the speaker is the "player"
    ~ speaker = ""
}

*[Enter]

-> round_1
=== round_1 ===
 #SFX: Ding
~ inThought = true

 #speaker:Narrator
 #expression:Default

(The elevator dings.)

(The doors slide open. Inside is a man — overweight, sweating, breathing just a bit too loudly. He looks up.)
~ inThought = false


 #speaker:Female
 #expression:Neutral
"Hi."

~ inThought = true
 #speaker:Narrator
  #expression:Default

(He barely responds. Just a nod.)
~ inThought = false
*[walk in] -> stuck_elevator

=== stuck_elevator ===
#SFX: High_heel_walking
~ inThought = true
( I step inside.)

#SFX: Elavator_door_close
(The doors close behind me with a heavy thud.)

#SFX: Click
#SFX: Screech
(I hear a sudden *click*—the familiar hum of movement... doesn't come.)

*[Presses the 'Open' button.] -> open_button_first

*[Tries the 'Emergency' button.] -> emergency_button_first





= open_button_first
#SFX: Elevator_button
(Nothing.)
*[Tries the 'Emergency' button.]  ->emergency_button

= emergency_button
#SFX: Elevator_button
(...no response.) 
-> stuck_elevator_continued




= emergency_button_first
#SFX: Elevator_button
(...no response.)
*[Presses the 'Open' button.] -> open_button

= open_button
#SFX: Elevator_button
(Nothing.) 
-> stuck_elevator_continued





= stuck_elevator_continued

 #speaker:Female 
#expression:Neutral
"What the...?"
 #speaker:Narrator
  #expression:Default

(Why isn't it working?)

#speaker:Male 
#expression:Neutral
"I think... we might be stuck." *quietly*

#speaker:Narrator
 #expression:Default
 #effect: shake
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
 #speaker:Narrator
  #expression:Default

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

~ inThought = true
 #speaker:Narrator
  #expression:Default

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
 #speaker:Narrator
  #expression:Default

(The air feels thick.)

(His eyes flick toward me now and then... Or am I imagining that?)

#speaker:Female
#expression:Neutral

"So… you think they’ll fix this soon?"

#speaker:Male
#expression:Neutral
"Shouldn’t take long."

~ inThought = true
 #speaker:Narrator
#expression:Default
(He’s closer now. )

(Just slightly. )

(Maybe I shifted back, maybe he forward... )

#speaker:Male
#expression:Neutral
"Ha..."

 #speaker:Narrator
 #expression:Default
 (Yea. He's definitely getting closer...)

-> escalation_choice





=== escalation_choice ===
//(He starts asking personal questions.)
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

~ inThought = true
 #speaker:Narrator
 #expression:Default

(I can’t breathe.)
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
 #expression:Default

(She lunges.)

(The elevator doors open. Chaos. Voices. Police.)

#speaker:Female
#expression:Neutral

"He—he was going to—!"

#speaker:Narrator
#expression:Default

(The screen flickers. CCTV footage plays: grainy, overhead. The girl stands tall. The man is hunched, small. There's no motion from him — until the moment she attacks...)

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
