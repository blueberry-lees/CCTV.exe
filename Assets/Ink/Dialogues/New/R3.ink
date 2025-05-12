// === ROUND 3 START ===
-> round3_intro

=== round3_intro ===
#pause_scene
#show_scene("elevator_dim")
#set_expression("neutral")

You step into the elevator once more. The air feels heavier today, the fluorescent lights casting a sickly hue.

#pause(1.0)

The doors begin to close, but a familiar hand halts their motion.

#pause(1.0)

He steps in.

-> describe_man_round3

=== describe_man_round3 ===
#show_character("Man", "smile_neutral")

His suit is immaculate, but there's a subtle smear on his cuff. His eyes meet yours, and for a fleeting moment, they seem... hollow.

MAN:
"Back again. Fate has a peculiar sense of humor, doesn't it?"

*   [forced smile]
    "Seems like it."
*   [uneasy]
    "Do you always ride this elevator?"
*   [silent]
    You nod, avoiding his gaze.
-
MAN:
"Elevators are like confessionals, don't you think? Small spaces where truths slip out."

-> elevator_ride_round3

=== elevator_ride_round3 ===
The elevator hums as it descends. The floor numbers flicker erratically.

MAN:
"Sometimes, I wonder if we're going down at all. Maybe we're just stuck, reliving the same moments."

You clutch your bag tighter, fingers brushing against something cold and metallic inside.

*   [inquire]
    "What do you mean by that?"
*   [defensive]
    "Are you implying something?"
*   [change subject]
    "Do you work in this building?"
-
MAN:
(smiling)
"Work? I suppose you could call it that. I observe, listen, understand."

His gaze intensifies, piercing through you.

MAN:
"Tell me, do you ever feel like you're being watched?"

*   [truthful]
    "Yes... lately, more than ever."
*   [lie]
    "No, not really."
*   [deflect]
    "That's a strange question."
-
MAN:
"Is it? Perhaps we're all just mirrors, reflecting each other's truths and lies."

-> delusion_build_up_round3

=== delusion_build_up_round3 ===
#show_expression("disturbed")

The elevator's lights flicker, and for a moment, the man's reflection doesn't match his movements.

A whisper echoes in your mind: "He's not real. End it."

You shake your head, trying to dispel the voice.

MAN:
"Are you alright? You seem... tense."

*   [confront]
    "Who are you really?"
*   [deflect]
    "Just tired, that's all."
*   [accuse]
    "You're lying to me."
-
MAN:
(chuckles)
"Aren't we all?"

-> climax_round3

=== climax_round3 ===
The elevator jerks to a halt between floors.

The lights dim.

Your hand moves on its own, reaching into your bag, fingers wrapping around the cold handle of a knife.

MAN:
"Sometimes, the only way to see the truth is to cut away the lies."

*   [stab] //kill him
    Without hesitation, you lunge forward, plunging the knife into his chest.
*   [hesitate] //scrached his cheek?
    You waver, but the voice in your head screams, and you act.
-
Blood splatters across the elevator walls.

The man's eyes widen in shock, then fade.

Suddenly, the elevator doors are pried open.

Police officers swarm in, weapons drawn.

OFFICER:
"Drop the weapon! Hands where we can see them!"

You release the knife, hands trembling.

As you're restrained, you glance at the man's lifeless body, a pool of blood spreading beneath him.

#show_expression("shocked")
#pause(2.0)

The elevator doors close.

Darkness.

-> END
