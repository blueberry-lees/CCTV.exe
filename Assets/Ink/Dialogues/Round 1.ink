VAR inThought = false
VAR speaker = ""

{inThought:
    // If inThought is true, don't show the speaker's name
    ~ speaker = ""
- else:
    // If inThought is false, the speaker is the "player"
    ~ speaker = "player"
}

-> round_1
=== round_1 ===

~ inThought = true
 #speaker:Narrator
(The elevator dings.)

(The doors slide open. Inside is a man — overweight, sweating, breathing just a bit too loudly. He looks up.)
~ inThought = false

 #speaker:Female
"Hi."

~ inThought = true
 #speaker:Narrator
(He barely responds. Just a nod.)
~ inThought = false
*[walk in] -> stuck_elevator

=== stuck_elevator ===

~ inThought = true
( I step inside.)

(The doors close behind me with a heavy thud.)

(I hear a sudden *click*—the familiar hum of movement... doesn't come.)

*[Presses the 'Open' button.] -> open_button_first

*[Tries the 'Emergency' button.] -> emergency_button_first



= open_button_first
(Nothing.)
*[Tries the 'Emergency' button.]  
->emergency_button


= emergency_button
(...no response.) 
-> stuck_elevator_continued



= emergency_button_first
(...no response.)
*[Presses the 'Open' button.] 
-> open_button


= open_button
(Nothing.) 
-> stuck_elevator_continued


= stuck_elevator_continued

~ inThought = true

(Why isn't it working?)

#speaker:Male
"I think... we might be stuck." *quietly*

~ inThought = true
#speaker:Female
(No. No, no, no...)

#speaker:Female
"You mean the elevator stopped?"

#speaker:Male
"Looks like it."

#speaker:Female
"..."

*   [Panic.] -> panic_mode
*   [Stay calm.] -> keep_composure





=== panic_mode ===
#speaker:Female
"This can't be happening!"

*[pound on the door]

 #speaker:Narrator
(I pound on the door. No sound from outside.)

...

#speaker:Male
"Hey... It's probably just a short. These things reset."

#speaker:Female
"You don’t know that..."

-> silence_start





=== keep_composure ===
#speaker:Female
"Okay. Okay. Maybe it’s temporary."

#speaker:Male
"Probably. These things reboot."

#speaker:Female
"Hopefully soon..."

-> silence_start






=== silence_start ===
#speaker:Male
"..."

* [Stay silent.]
#speaker:Female
"..."

(The silence stretches...)

#speaker:Male
"..."

->still_silent



=== still_silent ===

* [Still stay silent.]
#speaker:Female
"..."

-> interaction_start

=== interaction_start ===
 #speaker:Male
(He clears his throat.)

#speaker:Male
"You work on this floor?" 
* ["Yeah"] -> smalltalk_yes
* ["Don’t talk to me."] -> smalltalk_no




=== smalltalk_yes ===
#speaker:Female
"Yeah. Just started last week."

#speaker:Male
"Oh. Welcome to hell, then." *chuckles*

#speaker:Female
"..."

~ inThought = true
 #speaker:Narrator
(Was that supposed to be funny?)

-> unease_grows



=== smalltalk_no ===
#speaker:Female
"I’d prefer not to chat."

#speaker:Male
"...Alright then."

#speaker:Female
"..."

-> unease_grows




=== unease_grows ===
~ inThought = true
 #speaker:Narrator
(The air feels thick.)

(His eyes flick toward me now and then... Or am I imagining that?)

#speaker:Female
"So… you think they’ll fix this soon?"

#speaker:Male
"Shouldn’t take long."

~ inThought = true
 #speaker:Narrator
(He’s closer now. )

(Just slightly. )

(Maybe I shifted back, maybe he forward... )

#speaker:Male
"Ha..."

 #speaker:Narrator
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
"You okay? You’re... sweating a lot."

#speaker:Male
"Yeah, just—heat. Hate small spaces."

#speaker:Female
"Same..."

~ inThought = true
*[Something’s off.] -> accusation_made
*[Something’s off.] -> accusation_made
*[Something’s off.]-> accusation_made





=== accusation_made ===
#speaker:Female
"Don’t touch me!"

#speaker:Male
"What?! I didn’t—"

#speaker:Female
"Back off!"

~ inThought = true
 #speaker:Narrator
(I can’t breathe.)
(I reach into my bag—)
//play sfx

-> attack_decision




=== attack_decision ===
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1
*   [Pull the knife.] -> attack_ending_1





=== attack_ending_1 ===

#speaker:Female 
"Stay away from me!"

#speaker:Male
"Wait! Please—"

 #speaker:Narrator
(She lunges.)

(The elevator doors open. Chaos. Voices. Police.)

#speaker:Female
"He—he was going to—!"

#speaker:Narrator
(The screen flickers. CCTV footage plays: grainy, overhead. The girl stands tall. The man is hunched, small. There's no motion from him — until the moment she attacks...)

*[return to preview] -> return_to_preview_1




=== return_to_preview_1 ===
*[Launch review] ->replay

=replay
[FILE ALREADY ACCESSED. REPLAY?]

*   [Yes.] -> replay_round_2




=== replay_round_2 ===
[ROUND 2]
// Next segment to be scripted
->DONE
