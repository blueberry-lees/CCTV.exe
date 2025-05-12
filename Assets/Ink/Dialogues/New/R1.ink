VAR trust_level = 0
VAR look_away = false
// === ROUND 1 START ===
-> elevator_intro

=== elevator_intro ===
#pause_scene
#show_scene("elevator_dim")
#set_expression("neutral")

You step into the elevator, the soft hum of machinery buzzing faintly beneath your feet. It's cold. Sterile. Like the air is waiting for something to happen.

#pause(1.0)

The doors begin to close. But just before they shut—

#pause(1.0)

A hand stops them.

#pause(0.5)

A man steps in.

-> describe_man

=== describe_man ===
#show_character("Man", "smile_neutral")

He’s average. Tallish. Pale. Clean suit. Polished shoes.

But there’s something... off.

His eyes don’t quite match his smile. One lingers too long. The other darts away too quickly.

You feel it immediately. That gnawing twitch in your chest. The feeling again.

*   [look away] You keep your gaze on the floor. The fluorescent lights reflect in the polished steel.
    ~ look_away = true
    
*   [stare at him] You meet his eyes. Something sharp glints in them. Not metal. Something worse.
    ~ look_away = false

-
-> small_talk_start

=== small_talk_start ===
# show_expression("uneasy")

The elevator begins its slow descent.

The silence stretches.

Too long.

He finally speaks.

MAN:  
(pleasant)  
"Going down?"

You don’t answer right away. Something in your throat locks.

*   [nervous smile]
    "Y-yeah. Just... just heading down."

*   [bluntly]  
    "Yes."

*   [joke]  
    "Only way to go from the top, right?"
    ~ trust_level = 3

-
MAN:  
(chuckles softly)  
"Suppose so."  
"Funny thing, these little boxes. Tiny worlds. No escape. Just you and... whoever walks in."

He pauses. Like he’s letting the words hang in the air for too long.

Like he's watching them sink into your skin.

*   [awkward laugh]  
    "Yeah... elevators are weird."

*   [tense]  
    "What do you mean by that?"

*   [joking]  
    "Sounds like a setup for a horror movie."
    
-
MAN:  
"Horror... hmm. Maybe. Or just a moment where everything slips. When you realize you were never alone to begin with."

You laugh nervously, but it dies in your throat.

-> delusion_hint

=== delusion_hint ===
#show_expression("tense")

There’s something wrong.

His reflection in the elevator mirror behind him—it doesn’t move when he does.

You blink. Look again.

It’s back to normal.

Was it?

*   [rub your eyes] 
    You blink a few more times, heart thudding. Maybe you’re tired.

*   [look again]  
    You focus on the reflection. It stares back. Eyes too wide.

*   [step back slightly]  
    You shift away, gripping your bag tighter.
-
-> voice_in_head

=== voice_in_head ===
#show_expression("paranoid")

A whisper curls in your mind. Soft. Familiar.

???:  
"He’s not real. He’s not real. Not this time."

You shake your head. The whisper is gone.

The man’s still smiling. But his eyes... they flicker for just a moment.  
Black, like a void.

Then they’re normal again.

MAN:  
"You alright? You look pale."

*   [lie]  
    "Just tired. Didn’t sleep well."

*   [avoid]  
    "I’m fine."

*   [truth]  
    "Do you ever feel like someone’s lying to you? Even if they say all the right things?"
-
MAN:  
(smiles, pauses too long)  
"All the time."

-> ending_choice_1

=== ending_choice_1 ===
The elevator dings.

You’re almost at the lobby.

He steps closer. Just slightly. Almost imperceptibly.

But you notice.

MAN:  
"I think we'll be seeing more of each other."

*   [freeze]  
    You don't respond. Can't.
*   [respond]  
    "I... hope not."
*   [fake a smile]  
    "Looking forward to it."
    
    -

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
