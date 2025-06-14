// === ROUND 1 START ===

->Round_1
// tags contain 
//#speaker: Male, Female, Narrator
//#SFX:
//#expression:
//#character: Killer, You
//#effect

/* ---------------------------------

   Functions

 ----------------------------------*/
CONST INITIAL_SWING = 5

=== function swing_count(x) 
    ~ return (upness(x) + downness(x)) - 2

=== function swing_ready(x) 
    ~ return swing_count(x) >= 2

=== function raise(ref x)
    ~ x = x + 10

=== function lower(ref x)
    ~ x = x + 1 

=== function upness(x)
    ~ return x / 10

=== function downness(x)
    ~ return x % 10

=== function high(x)
    ~ return (1 * upness(x) >= downness(x) * 9)

=== function up(x)
    ~ return swing_ready(x) && (4 * upness(x) >= downness(x) * 6)

=== function down(x)
    ~ return swing_ready(x) && (6 * upness(x) <= downness(x) * 4)

=== function mid(x)
    ~ return (not up(x) && not down(x))
-
/* ---------------------------------

   Global Variables

 ----------------------------------*/

VAR speed = 0.05
VAR delay = 0.1


VAR UIVersion = 1

VAR trust = INITIAL_SWING
VAR delusion = INITIAL_SWING


//var for choices

VAR refuseToStab = false

VAR stabbed_status = ""
VAR confidence = 0


//
// Content
//
=== Round_1
~ UIVersion = 1
#character: off
#speaker: Narrator
#background: AllBlack
Which direction are you heading?
* [Up] Don’t want to run into your boss again. At least not today. 
-> Round_1  

* [Down] As if it’s not the only option.  

* [Stay] You’ve *just* finished work—why would you want to stay? 
-> Round_1  
    -
// elevator_intro  
#background: ElevatorOpen  #SFX: Ding  #speaker: Narrator  #speed: 0.1  
The elevator dings.

#ambient: BothersomeLift
#speed: 0.05  
You step in. That bothersome music hums in the background.

#SFX: CartoonWind  
The air’s cold—too much AC.

The doors begin to close. But just before they shut—

#background: pixelHand  #speed: 0.07  
A hand darts in.

#background: ElevatorOpen  #character: Killer  #expression: NeutralLook  #speed: 0.08  #delay: 0.4  
Someone steps in, nodding politely.

-> chapter_1


=== chapter_1
#background: Elevator #character: Killer #expression: NeutralLookAway #speed: 0.07 #delay: 0.4
The doors shut. You glance at the figure through the corner of your eye. 

He’s tallish, maybe late 20s. Clean-cut suit. Polished shoes. Looks pale. 

*   [look away] ->LookAway

*   [stare at him] 
->StarAtHim



=LookAway
#background look feet
#character:off
You shift your gaze to the screen. The elevator begins to descend.
~lower(trust)
    ~confidence = 0
-> chapter_2



=StarAtHim
 #character: Killer #expression: NeutralLook
You meet his eyes.

 #character: Killer #expression: SmileLook
He smiles casually.
~raise(trust)
~confidence = 1
    
    -> chapter_2




=== chapter_2

#character: Killer #expression: NeutralLookAway
#speaker: Narrator #speed: 0.07 #delay: 0.4

The silence lingers, slightly awkward...it's always like this with strangers. 

But suddenly, he speaks. 

#speaker: Male #character: Killer #expression:SmileLookT #speed: 0.08 #delay: 0.3
"Going down?"  


#speaker: Female
*   {confidence <= 0}[awkward] "Yeah... just heading home."
    ~ confidence = 0 
    ~ raise(trust)  
    #speaker: Male
    #character: Killer  #expression:  NeutralLookAwayT
    "Home, huh? Must be nice."
    **[Wdym?] "What's that suposed to mean?"
    **[Hby?] "Where’re you off to then?"
    -- 
    #Speaker:Male
    #character: Killer  #expression: Awkward
    "Ha... They roped me into delivering a few forms."
    
    #Speaker: Female
    "Oh..."

*   {confidence >= 0}[blunt]  "Only way to get out of this hell."
    ~ confidence = 1
    ~raise(delusion)
    #speaker: Male
    #character: Killer  #expression:  OneEyeCloseLaugh
    "True. So, what made you choose this job?"
    #speaker:Female
    "Only one that pays, really."
    #speaker:Male
    #character: Killer  #expression:  CloseEyeLaugh
    "Pfft. Guess there wasn’t much choice, was there?"
    #speaker:Female
    **[good 4 u]"Sounds like you had plenty of choices."
    **[and you?] "And you?"
    --
    #speaker:Male
    #character: Killer  #expression: Awkward
    "Choosing this job? No chance."
    
    

*   {confidence > 0}[rude] "What a stupid question."
    ~ confidence = 2
    ~ lower(trust)
    #speaker: Male
    #character: Killer  #expression: Speechless
    "HA. Sharp tongue. Let’s hope it doesn’t get you into trouble."
    #speaker:Female
    **[sarcastic] Oh, yeah, I'm so scared.
    #speaker:Male
    "..."
    #speaker:Female
    Sorry, rough day.
    #speaker:Male
    #character: Killer  #expression: Awkward
    "Haven't we all?"
    
    **[apologies] Sorry, rough day.
    #speaker:Male
    #character: Killer  #expression: Awkward
    "Haven't we all?"
    --
    
    
-
#character: Killer  #expression: SmileLookAway
#speaker: Female 
"..."
#character: Killer  #expression: NeutralLookAway
#speaker: Male
"..."
#speaker: Male
#character: Killer #expression:Doubt
"...Hey, don’t you think this elevator—"
"..."
"..."


#character: Killer  #expression: SmileLookAway   
"...never mind."

#speaker: Narrator
Perhaps he was about to say something important, but in the end, you don't really care enough to ask.


-> chapter_3


=== chapter_3

#character:off
#background: CheckMirror
#speaker:Narrator #speed: 0.06
You glance at the elevator mirror. The two of you stood there.
#background: AllBlack
You blink. 
#ambient: stop
#background: ReflectionStared
Then—his reflection is staring at you.


*   [blink]#background: ReflectionStared #background: LookCloser #background: IsGoneAgain
    ~ lower(delusion)
You blink a few more times. It’s gone again. 
   

*   [look closer] #background: LookCloser
    ~ up(delusion)
You squint at the reflection. It smiles at you.
   

*   [ignore] 
#background: Elevator2
   
You step back, casually adjusting your bag.
  ~ down(delusion)  
-

#ambient: BothersomeLift
#background: Elevator
#speaker: Male #character: Killer #expression:Concern
"You alright?"

#speaker: Female

*   [nod] "Yeah. Just tired."
    ~ up(trust)

*   [shrug] "Zoning out, I guess."
    ~ raise(trust) 
     ~ raise(delusion)

*   [I'm fine] "Fine. Mirrors just mess with me sometimes."
    ~ down(trust)
    ~ raise(delusion) 
-

#speaker: Male #character: Killer #expression:SmilePointUp
"I get that. Elevators can feel a bit... floaty."

#speaker: Narrator 
You smile faintly. He seems normal. Friendly, even.

#character: Killer #expression:HappyLookAway
It’s quiet again, but it doesn’t feel too awkward.

Just two people. 
Sharing space. 
Going down.

-> chapter_4

=== chapter_4

The elevator dings again.#SFX: Ding #speaker: Narrator #speed: 0.08
#Background: ElevatorThirdFloor
#character:off
You glance at the screen—third floor.

#speaker: Male #character: Killer #expression:SmileFists
"Well. Fun talk."
#speaker: Female  
* {(delusion > trust)} [quietly] "Mm." //{down(trust)} 
    ~ lower(trust)

* [neutral] "Take care." //{mid(trust)} 
    ~ mid(trust)

*  {(delusion < trust)}[polite] "See you around." //{up(trust)}
    ~ raise(trust)
-

#character: Killer #expression:HappyLookAway

#Background: ElevatorThirdFloorOpen
#speaker: Narrator
The doors open with a quiet sigh. 

#speed: 0.1  #delay:1
#Background:HeSmiled1
#character: off
#ambient:stop
He steps out. 

->hesmiled2
=hesmiled2
#Background:HeSmiled2
For a moment, he turns to glance back.

~ UIVersion = 2


#Background:HeSmiled3
And he smiled.

#Background:HeSmiled4
...
#Background:HeSmiled5
...
#Background:HeSmiled6
...
#Background:HeSmiled7
Just that.
#Background:AllBlack
You're alone again.


-> END


// === ROUND 2 START ===


=== ROUND_2 ===
#charcter:off
#background: AllBlack
#ambient:OUOLoop
<<<ENTERING UNFINISHED AREA, CONTINUE?
*[YEY]
*[NAY]
-
#background: ElevatorOpen2
You step into the elevator again. 

It feels... familiar.

The doors begin to close, but they hesitate—like they're waiting.

#background: HandOnDoorR3
A hand darts in. 
#background: AllBlack
He enters.


Everything'd the same.
Except.
You noticed there’s a faint crease at the collar. As if someone grabbed it.

His eyes meet yours. Still polite. Still warm. But you think you saw something else—gone now.

#speaker:Male 
"Ah. We meet again. Funny how elevators have such good memory."


*  [smile politely]
    "Or maybe we're just predictable." #speaker:Female
        ~ raise(trust)
*  [nervous chuckle]
    "I guess it’s that kind of morning." #speaker:Female
        ~ mid(trust)
*  [say nothing]
    #speaker:Narrator
    You manage a nod. Nothing more.#speaker:Female 
        ~ lower(trust)  // Withholding interaction this time

-

"I always liked elevators. They're like a neutral ground. Nowhere to go except where they take you." #speaker:Male


-> elevator_ride_round2

= elevator_ride_round2 
The elevator hums downward. #speaker:Narrator

The lights buzz faintly overhead. One of them flickers—not enough to be alarming, just enough to annoy.

"Do you think places remember us?" #speaker:Male


*   [curious] "You mean like... emotionally?" #speaker:Female
     ~ raise(trust)
*   [guarded] "That’s a strange question." #speaker:Female
    ~lower(trust)
*   [soft] "Maybe. Sometimes it feels that way."#speaker:Female
    ~ raise(trust)
    ~ raise(delusion)  // Opening up to uncanny ideas
-

"Buildings... routines... people. Maybe they don’t forget as much as we think." #speaker:Male

#speaker:Narrator
You glance at his reflection in the polished doors. For a second, it lags behind him. 

Your fingers find the edge of your bag.

You weren’t planning on carrying that today. But you are.


"You ever notice how time feels strange in here?" #speaker:Male


*   [uneasy] "It feels slow." #speaker:Female
    ~ raise(delusion)
*   [deflect] "Are you always this philosophical?" #speaker:Female
    ~lower(trust)
*   [tense] "Why do you keep asking things like that?" #speaker:Female
    ~ lower(trust)
    ~ raise(delusion)  // Combative tone often comes from paranoia
-



(smiling)
"Just passing the time. Conversations leave imprints too, don’t you think?" #speaker:Male



-> delusion_build_up

= delusion_build_up 
 #speaker:Narrator
The hum of the elevator deepens, almost like a growl. You blink, and for a heartbeat, the man isn’t in the reflection at all.

Just you. Alone.

Then he’s back.

A whisper—not in your ears, but behind your eyes:  
> "You already know. He doesn’t belong here."

You tighten your grip on the strap of your bag. Your knuckles ache.


"You alright? You look a little pale."#speaker:Male


*   [try to sound normal] "I'm fine. Just tired." #speaker:Female
    ~ lower(delusion)
    
    (light laugh)
    "We all are." #speaker:Male
    
*   [challenge him] "You always act like you know things." #speaker:Female
    ~ raise(delusion)
    ~ lower(trust)
    
    
    (light laugh)
    "Do I?" #speaker:Male
    
*   [direct] "What do you want from me?" #speaker:Female
    ~ raise(delusion)
    ~ lower(trust)
    
    
    (light laugh)
    "Sometimes, wanting isn't part of it." #speaker:Male

-
#speaker:Narrator
His smile doesn’t reach his eyes anymore. 

Something inside you feels like it’s tipping.

-> climax_round2

= climax_round2 


The elevator shudders. Stops between floors.#speaker:Narrator

Lights dim, shadows stretch across the walls.

You don’t remember making the decision. You just know your hand is already in the bag.

The handle is cold. Familiar.


"You've been holding on to that weight for a long time, haven’t you?" #speaker:Male


#speaker:Narrator

*   [act without thinking] You step forward and thrust the blade into him before you can stop yourself. 
         ~ raise(delusion)
            ~ lower(trust)
*   [hesitate, but obey the voice]
    <>You shake, frozen—but something pushes you, and the knife finds him anyway.
    
     ~ raise(delusion)
    ~ mid(trust)

-

#blood splash sound
The warmth of blood hits your face.

He stares at you—hurt, yes. But also... calm.

He sinks to the floor.

Then—

The doors wrench open. Harsh light floods in.

OFFICER:
"Drop it! Hands up! Now!"

Your fingers release the knife. It clangs against the floor.

They pull you down. Restrain you.

You look once more. He’s there, slumped and still.

But for just a moment, you swear—

There’s no blood at all.

#show_expression("blank")
#pause(2.0)

#speaker:Female
"...He wasn’t—"

#speaker:Narrator
The doors close. Everything vanishes into black.

-> END

=== Round_2_5
#speaker:Narrator

You remember the sound of the knife sinking in.

Not the scream. Not even the blood.

Just the dull, wet *resistance*.  
Like pushing through meat.

...

Then — nothing.

Except...

-> round_2_5_wake

=== round_2_5_wake ===
You're back in the elevator.

Or maybe you're *still* here.

The man stands beside you again. Same shirt. Same scent. Same stillness.

You try not to look at his side—where the blade should be.
No blood.
No wound.
No anything.

Your hand dips into the bag. You don’t remember deciding to do it. But it’s already happening.

The cold handle.

The weight of something final.

#speaker:Male
"You're not sure if this is real, are you?"

#speaker:Narrator
You freeze.

The voice in your head whispers—gentle, persuasive:

> "He's not real. Not anymore."

#speaker:Female
*   [pull away from the bag]
    #speaker:Narrator
    You try. You really try. Your hand won’t let go.
    
    > "You’re just delaying the truth."
    
    The elevator flickers again. His reflection blinks out.

    #speaker:Male
    (softly)  
    "You don’t have to do this."

    But his voice sounds wrong. Too clean. Too rehearsed.

    And something behind his smile is breaking.

    -> forced_action
*   [drop the knife]
    #speaker:Narrator
    You release it. Let it fall.

    But your body stays in motion—shoulders, knees, hands. Acting on some script you never wrote.

    The blade is already in your hand again.

    > "Good girl. You almost forgot."

    -> forced_action
*   [say “I don’t want this”]
    #speaker:Narrator
    Your voice shakes as you whisper it.

    But no one responds—not him, not the elevator, not the world.

    You’re alone with the voice. And it’s already made the decision.

    -> forced_action

= forced_action
#speaker:Narrator

The metal flashes. His chest rises once, sharply—then not again.

You feel the impact, the resistance, the warmth.

#blood splash sound
Blood hits the mirror.

#speaker:Male
(softly)  
"...I forgive you."

He slumps.

And you’re left holding the knife. Again.

The elevator doors wrench open. Blinding lights. Police shouting.

OFFICER:  
"Drop it! Hands up! Now!"

You drop the weapon.

They drag you to the floor.

You try to speak—to explain—but you don’t know how.

#pause(2.0)

You glance toward the man.

But the blood’s already gone.

#show_expression("blank")

#speaker:Female
"...I tried not to."

#speaker:Narrator
The doors slide shut.

Darkness follows.

-> END





=== Round_2_6
#speaker:Narrator
You thought it was over.  
Didn’t you?

But the air smells wrong.
The light stutters, like a memory buffering.

You're holding the knife again.

He's breathing softly beside you.  
Does he know what's coming?

-> round_2_6_reentry

=== round_2_6_reentry ===
You know this part.

You can hear your heartbeat again — the moment before it happens.

Almost like you're trapped *mid-stab*, replaying the hesitation over and over...

Your hand moves without thought, reaching into your bag, fingers curling around cold metal.

#speaker:Male
"...You're doing it again, aren’t you?"

He doesn’t step back. Doesn’t raise his voice.

#speaker:Narrator
Your fingers tighten. You try to stop—but the whisper in your mind returns: “End it.”

#speaker:Female
* [stab]
    #speaker:Narrator
    You drive the knife forward.

    He gasps—a soft sound, like surprise, not fear.

#speaker:Male
"...Still hurts, you know."

#speaker:Narrator
He looks down at the blade, blood soaking his shirt, his expression oddly calm. Like he remembers this, too.

The doors groan open.

Voices. Flashlights. Hands dragging you down.

#blood splash sound

OFFICER:
"Drop it! Now!"

#speaker:Narrator
The blade clatters to the floor. Your hands lift on their own. You're not sure who moved them.

You're pulled out. You glance back—he's slumped against the wall, eyes dull, a smear of red trailing downward.

#pause(2.0)

#speaker:Female
"He wasn’t supposed to be real…"

Darkness. Again.

-> END

=== ROUND_3 ===
#pause_scene
#show_scene("elevator_flicker")
#set_expression("unsteady")

#speaker:Narrator
The elevator dings open. Same floor. Same light twitching overhead like it’s struggling to stay alive.

But this time... something cracks in your chest. Like you're arriving late to something you never left.

#speaker:Narrator
You step in.

He’s already there. Still. Waiting.
His eyes meet yours.
And for a second—just a second—you think you see something crack behind them.

#show_character("Man", "smile_tired")

#speaker:Male
“Oh. You made it.”

#speaker:Narrator
Same smile. Same voice. But he looks… worn. The lines around his eyes deeper, the suit less pristine. There’s red dried into the fabric.

#speaker:Narrator
Your pulse jumps.

-> memory_check


= memory_check
    *   [I stabbed him.]
        It comes back in pieces. The weight of the knife. His voice as he fell. 
        You wrap your arms around yourself. He shouldn’t be standing. But he is.
        -> man_speaks

    *   [It wasn’t real.]
        It couldn’t be. The blood wasn’t there when they took you. Just... shadows. Tricks.
        But the scent of copper still clings to your thoughts.
        ~ raise(delusion)
        -> man_speaks

    *   [He deserved it.]
        He remembers. You can tell. And he’s not angry. Just... expectant.
        -> man_speaks

    *   [I’m losing grip…]
        Time bends here. Like the walls are too close. Your breath fogs. But it’s not cold.
        ~ high(delusion)
        -> man_speaks



= man_speaks
#speaker:Male
“You look pale. Again.”

#speaker:Narrator
He says it gently. Like someone who's lived this already.

    *   [Say nothing.] -> floor_approach
    *   [“I know who you are.”] -> accuse_path
    *   [“Why do you keep looking at me like that?”] -> paranoia_path


= accuse_path
~ lower(trust)
#speaker:Female
“I know who you are.”

#speaker:Narrator
Your voice is thin, but it cuts. His smile doesn’t fade, but something behind it stiffens.

#speaker:Male
"I doubt that. But I know who you think I am."

    *   [“Don’t play games with me.”]
        ~ lower(trust)
        #speaker:Narrator
        Your hands shake. You *want* him to be a monster. It makes this easier.
        -> floor_approach

    *   [Back off.] 
        -> floor_approach



= paranoia_path
~ raise(delusion)
#speaker:Female
"Why do you keep watching me?"

#speaker:Male
"Because you always do this. You always start to crack by the fourth floor."

    *   [“You're lying.”]
        -> accuse_path
    *   [“...Maybe I am.”]
        ~ raise(trust)
        #speaker:Narrator
        You press your palms to your temples. Maybe it’s you. Maybe he’s just some guy in the wrong place every time.
        -> floor_approach



= floor_approach
#speaker:Narrator
Floor 4. Almost there.

Your chest tightens. The metal in your pocket shifts slightly, like it knows too.

    *   [Grip the knife.]     -> knife_choice
    *   [Keep your hands still.] 
        ~ refuseToStab = true
        ~ raise(trust)
        #speaker:Narrator
        You keep your fists clenched. But you don’t reach for the knife.
        -> elevator_exit_peaceful



= knife_choice
You touch the knife. Familiar. Cold. Hungry.

    *   [Pull it.] -> knife_drawn
    *   [Leave it.] 
        -> choose_not_to_stab



= knife_drawn
#speaker:Narrator
You lunge. Again.

But this time, his hand brushes yours. Late—but not surprised.

#speaker:Male
“You always come back to this.”

    *   [Finish it.]  
        ~ stabbed_status = "noHesitate"
        -> stab_scene

    *   [Hesitate.]
        ~ stabbed_status = "hesitate"
        -> stab_struggle



= stab_scene
#speaker:Narrator
You plunge the knife into his side. He gasps—but it’s not shock. It’s something else. Familiarity.

#speaker:Male
{ stabbed_status:
  - "hesitate": "...Still not sure if I’m real, huh?"
  - "noHesitate": "Not even gonna hesitate, I guess."
}

...Every time.
    You just keep coming back.

#speaker:Narrator
You pull away. His body slides down the wall. No resistance.

-> elevator_exit_bloody



= stab_struggle
#speaker:Narrator
You both freeze. The knife falls. Neither of you speak.

You breathe like you ran a mile. He watches you like he's seen this movie before.

-> elevator_exit_peaceful



= choose_not_to_stab
~ raise(trust)
#speaker:Narrator
You release the blade. Meet his gaze. He tilts his head.

#speaker:Male
"...That’s new."

    *   [Say nothing.] 
        -> elevator_exit_peaceful
    *   [“You don’t deserve to die. Yet.”]
        ~ raise(delusion)
        -> elevator_exit_peaceful
    *   [“I’m done playing this game.”]
        ~ lower(delusion)
        -> elevator_exit_peaceful



=== elevator_exit_peaceful ===
~ refuseToStab = true
#speaker:Narrator
The elevator dings. The doors ease open.

But this time, it doesn't feel like escape. It feels like waking up—too soon or far too late.

#speaker:Narrator
Warm light spills in. Distant voices hum behind the walls. The world feels real again… but thinner. Like a memory stretched too far.

You hesitate.

He doesn’t move.

TODO: 
He doesn’t flinch. Doesn’t speak right away.  
Like he’s counting how many times you’ve done this.

#speaker:Male
“Until next time.”

#speaker:Narrator
No threat. No warmth.

Just inevitability. A loop tied tight.

You step out. The cold air bites sharper than before.

Behind you, the doors close. You don’t turn back.

-> DONE


=== elevator_exit_bloody ===
~ refuseToStab = false
#speaker:Narrator
The elevator dings. The doors creak open.

But the light outside is wrong. Warped. Like it passed through smoke.

Your hands drip red. So does the floor.

#speaker:Narrator
You step out anyway. Each footfall smears another piece of him into the world outside.

He slumps in the corner. Eyes wide. Still. But watching.

#speaker:Male
“...”

TODO:
He doesn’t flinch. Doesn’t speak right away.  
Like he’s counting how many times you’ve done this.

#speaker:Narrator
There’s no scream. No justice. No peace.

Just silence.  
And the gnawing certainty that next time, he’ll still be there.

-> DONE



-> SPLIT_ENDING

//so in truth, the man already died in round 2 when the player(woman) first killed him, so when she sees the man again, it has her delusionating his existance. So the elevator turned into a space in her mind, a space created by her. 

//thats why, in end A - The Innocent, she felt so guilty about killing the man that, she imagine herself being killed, so she can be the innocent one.

//That's why, in end B - The Justified, she act as if she's the one in right, made him the villain, to justify her action.
//That's why, in end C - The Monster, she felt so scared to see him 'alive' that, she think he's back for revenge, and so she's lost it, and became a monster.
//Tha's why, in end D - The Denial, she wish to escape, to disappear from this world, so she denied her own existance, her mind left in void, forever. 

=== SPLIT_ENDING ===
{
    - up(trust) && down(delusion)&& refuseToStab == true:
        -> END_A_INNOCENT // The Innocent – she's got killed by the man. she was just scared, not wrong
        
    - down(trust) && down(delusion) && refuseToStab ==false:
        -> END_B_JUSTIFIED // The Justified – she stopped a killer, but why did he wants to kill her in the first place?

    - down(trust) && up(delusion) && refuseToStab ==false:
        -> END_C_MONSTER // The Monster – she’s mentally unstable and violent and killed the man again

    - up(trust) && up(delusion)&& refuseToStab == true:
        -> END_D_DENIAL // Denial – 

}


//I want to use this at all the begginng of endings, but where should I put this? 
TODO: He doesn’t flinch. Doesn’t speak right away.
Like he’s counting how many times you’ve done this.


// === ROUND 4 START ===

=== END_A_INNOCENT
#pause_scene
#show_scene("elevator_bloodstained")
#set_expression("numb")



You step into the elevator again.

But this time... something’s wrong.

The light flickers red for a split second. Dried blood stains the metal wall behind you.

Your chest tightens.


The elevator doors begin to close—  
—but they hesitate. Almost as if the system itself is… unsure.

Then, he enters.
->describe_man_round4

= describe_man_round4 
# show_character("Man", "neutral_calm")

His smile is faint. Almost pitying.

There’s a new scar across his cheek. A thin line. A memory.

#speaker:Male
"Back again."

#speaker:Narrator
You swallow hard. The air feels thick, like breathing through gauze.

#speaker:Female
*   [guarded] "Why do I always end up here?"
*   [guilt-ridden]  "I-I didn’t mean to hurt you last time…"
*   [numb] #speaker: Narrator 
<>You said nothing. Just stare.

-
#speaker:Male  
(slowly)  
"Funny, isn’t it? How cycles repeat. Even broken things can loop."
#speaker:Narrator
He says as he points towards the screen. 

-> elevator_ride_round4

= elevator_ride_round4 

#speaker:Narrator
The elevator descends—only, the numbers are wrong. They tick **up**, then **back down**, then flickers out completely.

You watch him in the mirrored wall. But something’s wrong—  
His reflection doesn't blink when he does.

Your stomach knots.

#speaker:Male
"You know, they told me you weren’t well."

#speaker:Female
*   [confused]
    "Who did?"
*   [angry]
    "They don’t know anything about me!"
*   [guilt]  
    "Maybe they’re right…"
-
#speaker:Male
(smiling thinly)  
"You didn’t see me clearly… but that wasn’t your fault. Your mind’s a noisy place. Full of smoke and broken glass."

-> self_doubt_builds

= self_doubt_builds 
#show_expression("tremble")

#speaker:Narrator
Your hand itches. You touch your bag—  
—but freeze.

#colour in red
There's no knife this time.

Only cold air.

Your vision blurs. You blink—  
—for a moment, the man’s skin seems to flicker. Pale and clammy. Lips split with blood. Eyes glassy. Dead.

You blink again.

He’s alive. Calm. Watching.

#speaker:Male
"What do you see when you look at me?"

#speaker:Female
*   [honest]  "I don’t know anymore. You’re… not what I thought."
*   [angry]  "You’re a liar. You’ve always been lying."
*   [scared] "I thought you were dangerous. But now I think I’m wrong."

-

#speaker:Male
#(whispering)
"..."
"Sometimes-"
"the greatest danger..."
"is the one holding the blade."

-> emotional_breakdown

= emotional_breakdown 
# show_expression("breaking")

You grip the rail. Knuckles white.  
Everything in your head is screaming.

You see flashes:  
Blood-soaked hands. Sirens.  
The man gasping on the floor.  
Your reflection laughing when you weren’t.

You fall to your knees.

+   [desperate] "W-What have I done...?"
+   [pleading]  "I didn’t want to hurt anyone…"
+   [lost]"Am I even real?!"
-
#speaker:Male
"You’re real enough."

-> final_confrontation

= final_confrontation 
#speaker:Narrator
...
Then elevator stops.  
The lights flicker violently.

The man steps closer. 
Too close.

You back up—  
—but the wall meets you. There’s nowhere left.

#speaker:Male
"You let your instint guide you. But instincts lie, don’t they?"

"They twist fear into truth. And truth into knives."

#speaker:Narrator
He leans in.

#speaker:Male  
"But what if you were right the first time?"

#speaker:Female

*   [shaking]
    "What do you…?"
*   [tense]
    "Stop talking like that…"
*   [resigned]  
    "Then… I’ll die knowing I deserved it."
-

#speaker:Narrator
His hand moves to his jacket.

He pulls out something silver.
# show_expression("terror")

You try to scream—  
but there’s no time.


->death_scene
= death_scene 
# show_effect("stab_sound")
#show_expression("pain_shock")

Pain erupts in your neck.  
Warmth spills down your collar.

Your knees buckle.

You collapse to the floor, the world spinning.

The man kneels beside you, tilting your face.

His expression is gentle.

#speaker:Male
"Shhh. You were never supposed to see the whole picture."

#speaker:Narrator
He strokes your hair as your vision dims.

#speaker:Male
"So sleep now."

#speaker:Narrator
The your vision turned black.

-> main_ending



= main_ending
# show_scene("elevator_blackout")
#play_audio("flatline_hum")

A newspaper headline flashes across the screen:

**"ELEVATOR KILLER CLAIMS FINAL VICTIM – Mentally Unstable Woman Dies in Final Encounter"**

Then static.

Then silence.

-> END





===END_B_JUSTIFIED ===

# (The elevator is silent. The air hums with tension. The woman’s hands twitch by her sides.)

WOMAN: (internal) This is it. The final loop. I don’t know how I know, but... it feels different this time.

WOMAN: (internal) The way he's standing. How still he is. As if he's waiting.

# (He looks at her—no smirk, no charm, just quiet.)

MAN: You’re quieter today.

WOMAN: (hesitant) Maybe I’m listening better now.

MAN: (soft chuckle) And what are you listening to?

WOMAN: (internal) He’s trying to bait me again. That voice—calm, measured, too careful. Like he’s stepping around landmines.

-> player_choice_listening

= player_choice_listening 
*   "The lies between your words." 
    -> listening_lies
*   "My heartbeat." 
    -> listening_heartbeat
*   "The truth I've ignored too long." 
    -> listening_truth

= listening_lies 
WOMAN: The lies between your words.

MAN: (smiling) Ouch. That's cold.

WOMAN: I’ve had four rounds to learn your rhythm. The spaces you leave say more than what you say out loud.

-> continue_end


= listening_heartbeat
WOMAN: My heartbeats.

MAN: Huh. Introspective today.

WOMAN: I had to learn it the hard way. Listening to my thoughts almost killed me. Now I listen to what’s really there.

-> continue_end

= listening_truth 

#speaker:Female
"The truth I’ve ignored too long." 

#speaker: Male
(leans closer) "And what truth is that?"

#speaker:Female
"That I can be wrong."
"And that you’re not who I made you out to be." 
"..."
"You’re worse."

-> continue_end
= continue_end 

# speaker:Narrator
Suddenly, you saw something flickered at the edge of your sight.
Something that wasn't there before...
You inched your hand towards that thing, just a twitch at a time.

He’s waiting. Just like before. But this time... 
You're not frozen.

#speaker:Male
"That look... you've caught on to something I missed. What is it?"

This is it. 

#speaker:Male
"Why aren’t you saying anything? What are you—?!"


-> emergency_brake

=emergency_brake 

#speaker:Narrator
You grab the emergency brake and yank it down.

# (Loud alarm. Lights flicker. The elevator shakes to a halt.)

#SFX: (Alarm blares. Lights flicker. The elevator jerks to a stop.)

He loses it. <>
#speaker:Male
"What the hell did you just do?!"


#speaker: Female
I stopped imagining.


# speaker: Narrator
The doors are forced open. Flashlights cut through the dark.
POLICE OFFICER: Hands where we can see them!

He reaches into his coat—pulls out something silver.

POLICE OFFICER: DROP IT!

He hesitates... then lets it fall.
It hits the ground—sharp. A scalpel.

The sight of it gave you chills.
It wasn’t just in your head.
#speaker:Female
(breathing hard) It was real… all of it.

You try to move. But your legs gave out.

You looked back—
He stares at you, no longer smiling.

-> aftermath_awake

= aftermath_awake 

# (Black screen. Soft piano music.)

#speaker:Narrator
Later, in a quiet hospital corridor—

#speaker:Female
THERAPIST (V.O.): You did the right thing. You broke the pattern.

THERAPIST (V.O.): The delusions will try to return. But so will the truth, if you let it.

And so you left the room.
You run your hand along the wall as you walk through the hallway, grounding yourself.

And when you reach the end of the hallway, you saw an elevator, and the emergency exit next to it. 
*[Take the elevator] No, you will never enter that shit ever again in your life time.
+[Take the mergency exit]You took the emergency exit. There's a lot of staircase. 
-
But you don't care. 

So you stepped in.

#(Switch to cctv, this time, in the emergency exit. Shows a woman walked in through the door and took the staircase to go downstairs. And this time - a girl follows her. The girl noticed the cctv, did a 'shhh' gesture, and broke the cctv. Story ends.


-> END



=== END_C_MONSTER ===

# scene_elevator_end_round_4

# (The elevator is nearly at the top. The hum is louder now. She’s staring at the man again—trembling, but not from fear.)

WOMAN: (internal) I know what I saw. I know what he is.

MAN: You're looking at me like you want to say something.

WOMAN: Maybe I do.

MAN: Well, speak your truth.

WOMAN: (internal) Truth. Heh. They keep asking me what’s real. But none of them were in this elevator.

-> player_choice_truth

= player_choice_truth 
"You were going to kill me, weren’t you?" 

    -> delusion_kill_me
"You always smile when you lie." 

    -> delusion_smile_lie
"I saw you. In every version." 

    -> delusion_saw_you

= delusion_kill_me 
WOMAN: You were going to kill me, weren’t you?

MAN: (chuckles) That’s a bold claim.

WOMAN: I don’t need evidence. I remember the blood. I remember the fear.

-> continue_delusion

= delusion_smile_lie 
WOMAN: You always smile when you lie.

MAN: You’re imagining things.

WOMAN: That’s what they all say. But I see past it. I see you.

-> continue_delusion

= delusion_saw_you 
WOMAN: I saw you. In every version.

MAN: Maybe you dreamed me.

WOMAN: No. You were real. You always were. The knife. The blood. Your voice in the dark.

-> continue_delusion

= continue_delusion 

# (The hum of the elevator is rising in pitch. Her hand slides into her bag.)

WOMAN: (internal) They’ll never understand. They’ll think I’m mad again. That I need help.

WOMAN: But I don’t. I just need to end this.

MAN: What are you doing?

# (She pulls the knife out slowly. Her reflection in the metal gleams.)

WOMAN: Setting myself free.

# (She lunges forward—stabs—)

SFX: [Sickening crunch. Blood splatters the elevator wall.]

# (He slumps forward, gurgling, eyes wide.)

MAN: (choked whisper) You're... wrong...

# (The doors open. Screams. Police shouting.)

POLICE OFFICER: DROP THE KNIFE! DROP IT NOW!

# (But you just stands there. Covered in blood. Smiling.)

You're finally awake.
WOMAN: Ha..HAHA...AH HAHAHA...ha...
#cut to black

-> aftermath_delusion

= aftermath_delusion 

# (Black screen. Soft ambient noise—muffled voices, flickering lights.)

In a padded room. A woman sits facing a mirror that doesn’t reflect her.
She's being restrained, tied up with layers of straps and clothings.
As if she were a bomb. 

THERAPIST (V.O.): You don’t remember, do you?

WOMAN: What do you mean? I remember everything.

THERAPIST (V.O.): ...There was no man.

# (The woman turns to the mirror.)

WOMAN: He was real.

THERAPIST (V.O.): We found no one. No victim. Only you. Covered in blood.

WOMAN: ...Ha. 

WOMAN: (internal) They’re lying. Just like he did. Just like everyone does.

# (The mirror flickers. For just a second, the man’s face appears in the reflection—smiling.)

-> END





=== END_D_DENIAL ===



# (The elevator ride is slow. The lights flicker again. Her breathing is shallow, but steady.)

WOMAN: (internal) Maybe I was wrong. Maybe I imagined it. He hasn’t done anything. Afterall.

# (The man stands at the far end, leaning against the rail. Calm. Watching her.)

MAN: You’re quiet today.

WOMAN: Just tired.

MAN: Tired of being afraid?

WOMAN:...

-> denial_choice_approach

 = denial_choice_approach 
"I just want to get out of here." 

    -> denial_escape
"This is just in my head, right?" 

    -> denial_doubt
Stay silent. 

    -> denial_silent

= denial_escape 
WOMAN: I just want to get out of here.

MAN: (smiles) That makes two.

WOMAN: Please...stop talking. Just let this be over with.

-> continue_denial

= denial_doubt 
WOMAN: This is just in my head, right?

MAN: Does it matter?

WOMAN: It does to me.

MAN: Then pretend it is. Pretend everything’s fine.

-> continue_denial

= denial_silent 
# (She doesn't speak. Just clutches her bag tighter.)

MAN: Still pretending I’m not here?

WOMAN: (internal) If I just ignore him, he can’t hurt me. Right?

-> continue_denial

= continue_denial 

# (The elevator dings. Floor 14. Still going. The air is thick. She shifts her weight.)

WOMAN: (internal) One more minute. That’s all. Just get through one more minute.

MAN: You ever wonder what happens when we hit the top?

WOMAN: No.

MAN: You should.

# (Suddenly, the elevator *jerks violently*. Lights snap off. Emergency red light glows.)

SFX: [Mechanical groaning. A rising shriek in the metal.]

WOMAN: Wh-What’s happening?!

# (The man is gone.)

WOMAN: No. No no no no—

# (She turns — he’s now standing *right beside her*.)

MAN: You should’ve faced it.

WOMAN: What… what are you?

# (He smiles — mouth stretching impossibly wide. His eyes *bleed black*.)

MAN: Just a part of you.

# (She tries to scream. He places a finger on her lips.)

MAN: Shhh. This is your choice. You asked for it.

# (He plunges something into her chest. Not a knife — just *darkness*. It spreads like ink.)

SFX: [Heartbeat slows. The elevator doors open to nothing — a void.]

-> aftermath_denial

= aftermath_denial 

# (Black. Then faint static. A flickering video feed.)

NARRATOR (V.O.): Police investigation report. Elevator 17. No signs of struggle. No victim. No assailant.

DISPATCH (V.O.): Last known footage shows a woman standing alone. Smiling. Talking to herself.

# (The screen flickers to *CCTV footage* — she’s speaking, gesturing… to no one.)

WOMAN (CCTV): (murmuring) I just want to go home.

# (Static.)

NARRATOR (V.O.): No trace of her ever found.

# (Final flash: A red-lit elevator. Empty… except for a *bloody handprint* on the mirror.)

-> END





=== END_NEUTRAL ===
THIS IS NEUTRAL ENDING

->END
