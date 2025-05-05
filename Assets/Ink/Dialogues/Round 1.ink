// tags contain 

//#speaker:

//#SFX:

//#expression:

//#effect

-> round_1

=== round_1 ===

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

*[walk in] -> stuck_elevator


=== stuck_elevator ===

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
#background: Elevator
#background: ElevatorDark
#background: Elevator

#speaker:Narrator

You hear a sudden *click*—the familiar hum of movement... doesn't come.

#speaker:Female

#expression:InnerThought

(???)

#speaker:Narrator

You walk back to check the elevator panel.

*[Presses the 'Open' button.] -> open_button_first

*[Tries the 'Emergency' button.] -> emergency_button_first

= open_button_first

#SFX: Elevator_button

#speaker:Narrator

Nothing.

*[Tries the 'Emergency' button.]  ->emergency_button

= emergency_button

#SFX: Elevator_button

#speaker:Narrator

...no response. 

-> stuck_elevator_continued

= emergency_button_first

#SFX: Elevator_button

#speaker:Narrator

...no response.

*[Presses the 'Open' button.] -> open_button

= open_button

#SFX: Elevator_button

#speaker:Narrator

Nothing.

-> stuck_elevator_continued

= stuck_elevator_continued

#speaker:Female 

#expression:Neutral

"What the...?"

#speaker:Female
#expression:LightShadow

(Why isn't it working?)


#speaker:Male
#expression:Shadow
"I think... we might be stuck." *quietly*

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

*   [Panic.] -> panic_mode

*   [Stay calm.] -> keep_composure

=== panic_mode ===

#speaker:Female

#expression:Neutral

"This can't be happening!"

*[pound on the door]

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

-> silence_start

=== keep_composure ===

#speaker:Female

#expression:Neutral

"Okay. Okay. Maybe it’s temporary."

#speaker:Male
#expression:Shadow

"Probably. These things reboot."

#speaker:Female

#expression:Neutral

"Hopefully soon..."

-> silence_start

=== silence_start ===

#speaker:Male
#expression:Shadow

"..."

* [Stay silent.]

#speaker:Female

#expression:Neutral

"..."

#speaker:Narrator

#expression:Default

(The silence stretches...)

#speaker:Male
#expression:Shadow

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
#expression:Shadow
"Ehem..."

#speaker: Narrator
He clears his throat.

#speaker:Male
#expression:Shadow

"You work on this floor?" 

* ["Yeah"] -> smalltalk_yes

* ["Don’t talk to me."] -> smalltalk_no

=== smalltalk_yes ===

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

-> unease_grows

=== smalltalk_no ===

#speaker:Female

#expression:Neutral

"I’d prefer not to chat."

#speaker:Male
#expression:Shadow

"...Alright then."

#speaker:Female

#expression:Neutral

"..."

-> unease_grows

=== unease_grows ===

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

#speaker:Female
#expression:LightShadow

(Just slightly. )


#speaker:Male
#expression:Shadow

"Ha..."

#speaker:Female

#expression:InnerThought

(...Why is he getting closer?)

*[Something’s off.] -> escalation_choice

*[Something’s off.] -> escalation_choice

*[Something’s off.] -> escalation_choice




=== escalation_choice ===

*    ["Why are you sweating so much?"]

-> awkward_tension

*    ["Don’t touch me!"]

-> accusation_made

=== awkward_tension ===

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

*[Something’s off.] -> accusation_made

*[Something’s off.] -> accusation_made

*[Something’s off.]-> accusation_made

=== accusation_made ===

#speaker:Female

#expression:Neutral

"Don’t touch me!"

#speaker:Male
#expression:Shadow

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
#expression:LightShadow

"Wait! Please—"

#speaker:Narrator

She lunges.

#speaker:Narrator

[*Squelch*]

#speaker:Narrator

The elevator doors open. Chaos. Voices. Police.

#speaker:Female

#expression:Neutral

"He—he was going to—!"

#speaker:Narrator

The screen flickers. CCTV footage plays: grainy, overhead. The girl stands tall. The man is hunched, small.

#speaker:Narrator

There's no motion from him — until the moment she attacks...

...

->DONE

