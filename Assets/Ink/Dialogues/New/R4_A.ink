// === ROUND 4 START ===
-> round4_intro

=== round4_intro ===
-> pause_scene
-> show_scene("elevator_bloodstained")
-> set_expression("numb")

You step into the elevator again.

But this time... something’s wrong.

The light flickers red for a split second. Dried blood stains the metal wall behind you.

Your chest tightens.

-> pause(1.2)

The elevator doors begin to close—  
—but they hesitate. Almost as if the system itself is… unsure.

Then, he enters.

=== describe_man_round4 ===
-> show_character("Man", "neutral_calm")

His smile is faint. Almost pitying.

There’s a new scar across his cheek. A thin line. A memory.

MAN:  
"Back again."

You swallow hard. The air feels thick, like breathing through gauze.

*   (guarded)  
    "Why do I always end up here?"
*   (guilt-ridden)  
    "I didn’t mean to hurt you last time…"
*   (numb)  
    You say nothing. Just stare.

MAN:  
(slowly)  
"Funny, isn’t it? How cycles repeat. Even broken things can loop."

-> elevator_ride_round4

=== elevator_ride_round4 ===
The elevator descends—only, the numbers are wrong. They tick **up**, then **back down**, then flicker out completely.

You watch him in the mirrored wall. But something’s wrong—  
His reflection doesn't blink when he does.

Your stomach knots.

MAN:  
"You know, they told me you weren’t well."

*   (confused)  
    "Who did?"
*   (angry)  
    "They don’t know anything about me!"
*   (guilt)  
    "Maybe they’re right…"

MAN:  
(smiling thinly)  
"You didn’t see me clearly… but that wasn’t your fault. Your mind’s a noisy place. Full of smoke and broken glass."

-> self_doubt_builds

=== self_doubt_builds ===
-> show_expression("tremble")

Your hand itches. You touch your bag—  
—but freeze.

There's no knife this time.

Only cold air.

Your vision blurs. You blink—  
—for a moment, the man’s skin seems to flicker. Pale and clammy. Lips split with blood. Eyes glassy. Dead.

You blink again.

He’s alive. Calm. Watching.

MAN:  
"What do you see when you look at me?"

*   (honest)  
    "I don’t know anymore. You’re… not what I thought."
*   (delusional)  
    "You’re a liar. You’ve always been lying."
*   (scared)  
    "I thought you were dangerous. But now I think I’m wrong."

MAN:  
(whispering)  
"Sometimes the greatest danger... is the one holding the blade."

-> emotional_breakdown

=== emotional_breakdown ===
-> show_expression("breaking")

You grip the rail. Knuckles white.  
Everything in your head is screaming.

You see flashes:  
Blood-soaked hands. Sirens.  
The man gasping on the floor.  
Your reflection laughing when you weren’t.

You fall to your knees.

*   (desperate)  
    "Why did I do it?"
*   (pleading)  
    "I didn’t want to hurt anyone…"
*   (lost)  
    "Am I even real?"

MAN:  
"You’re real enough."

-> final_confrontation

=== final_confrontation ===
The elevator stops.  
The lights flicker violently.

The man steps closer. Too close.

You back up—  
—but the wall meets you. There’s nowhere left.

MAN:  
"You let your gut guide you. But guts lie, don’t they?  
They twist fear into truth. And truth into knives."

He leans in.

MAN:  
"But what if you were right the first time?"

*   (shaking)  
    "What do you mean…?"
*   (tense)  
    "Stop talking like that…"
*   (resigned)  
    "Then… I’ll die knowing I deserved it."

-> show_expression("terror")

His hand moves to his jacket.

He pulls out something silver.

You try to scream—  
—but there’s no time.

=== death_scene ===
-> show_effect("stab_sound")
-> show_expression("pain_shock")

Pain erupts in your neck.  
Warmth spills down your collar.

Your knees buckle.

You collapse to the floor, the world spinning.

The man kneels beside you, tilting your face.

His expression is gentle.

MAN:  
"Shhh. You were never supposed to see the whole picture."

He strokes your hair as your vision dims.

MAN:  
"Sleep now."

The elevator lights go black.

-> END_ROUND_4_MAIN_ENDING

=== END_ROUND_4_MAIN_ENDING ===
-> show_scene("elevator_blackout")
-> play_audio("flatline_hum")

A newspaper headline flashes across the screen:

**"ELEVATOR KILLER CLAIMS FINAL VICTIM – Mentally Unstable Woman Dies in Final Encounter"**

Then static.

Then silence.

-> END
