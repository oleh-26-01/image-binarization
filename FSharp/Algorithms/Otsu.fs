namespace ImageProcessing

module Functional =
    let Histogram2 (pixels: byte[]) : int[] =
        let histogram = Array.zeroCreate<int> 256
        pixels |> Array.iter (fun x -> histogram.[int x] <- histogram.[int x] + 1)
        histogram

    let Histogram (pixels: byte[]) : int[] =
        let mutable histogram = Array.zeroCreate<int> 256
        histogram <- pixels |> Array.fold (fun acc x -> acc.[int x] <- acc.[int x] + 1; acc) histogram
        histogram

    let ThresholdingOtsu2 (pixels: byte[]) : byte =
        let nbins = 256
        let histogram = Histogram pixels

        let p = Array.zeroCreate<double> histogram.Length
        for i = 0 to histogram.Length - 1 do
            p.[i] <- double histogram.[i] / double pixels.Length

        let sigmaB = Array.zeroCreate<double> nbins

        for t = 0 to nbins - 1 do
            let mutable qL = 0.0
            let mutable qH = 0.0
            for i = 0 to t - 1 do
                qL <- qL + p.[i]

            for i = t to nbins - 1 do
                qH <- qH + p.[i]

            if qL = 0.0 || qH = 0.0 then
                ()
            else
                let mutable miuL = 0.0
                let mutable miuH = 0.0
                for i = 0 to t - 1 do
                    miuL <- miuL + (double i * p.[i])

                for i = t to nbins - 1 do
                    miuH <- miuH + (double i * p.[i])

                miuH <- miuH / qH
                miuL <- miuL / qL

                sigmaB.[t] <- qL * qH * (miuL - miuH) * (miuL - miuH)

        let mutable threshold = 0
        let mutable max = sigmaB.[0]
        for i = 1 to sigmaB.Length - 1 do
            if sigmaB.[i] > max then
                max <- sigmaB.[i]
                threshold <- i

        byte threshold

    let ThresholdingOtsu (pixels: byte[]) : byte =
        let nbins = 256
        let histogram = Histogram pixels
        let totalPixels = double pixels.Length

        let probabilities =
            Array.map (fun count -> double count / totalPixels) histogram

        let sigmaB =
            Array.init nbins (fun t ->
                let qL = Array.sum <| Array.sub probabilities 0 t
                let qH = Array.sum <| Array.sub probabilities t (nbins - t)
                if qL = 0.0 || qH = 0.0 then 0.0
                else
                    let miuL = Array.mapi (fun i p -> double i * p) <| Array.sub probabilities 0 t |> Array.sum
                    let miuH = Array.mapi (fun i p -> double (t + i) * p) <| Array.sub probabilities t (nbins - t) |> Array.sum
                    let miuL = miuL / qL
                    let miuH = miuH / qH
                    qL * qH * (miuL - miuH) * (miuL - miuH)
            )

        let threshold = 
            sigmaB |> Array.mapi (fun i v -> i, v) |> Array.maxBy snd |> fst

        byte threshold

    let Binarize (pixels: byte[], threshold: byte) : byte[] =
        pixels |> Array.map (fun x -> if x > threshold then 255uy else 0uy)

    let BinarizeOtsu(pixels: byte[]) =
        let threshold = ThresholdingOtsu pixels
        Binarize (pixels, threshold)