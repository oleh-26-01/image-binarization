namespace Functional

open System

module Sauvola =
    let CalculateIntegralImage(pixels: byte[], width: int, height: int, transform: bool) =
        let integralImage = Array2D.zeroCreate<int> height width

        for y = 0 to height - 1 do
            for x = 0 to width - 1 do
                let mutable pixelValue = int pixels.[y * width + x]
                if transform then pixelValue <- pixelValue * pixelValue
                integralImage.[y, x] <- pixelValue
                if x > 0 then integralImage.[y, x] <- integralImage.[y, x] + integralImage.[y, x - 1]
                if y > 0 then integralImage.[y, x] <- integralImage.[y, x] + integralImage.[y - 1, x]
                if x > 0 && y > 0 then integralImage.[y, x] <- integralImage.[y, x] - integralImage.[y - 1, x - 1]

        integralImage

    let MeanStdIntegralImage(pixels: byte[], width: int, height: int, windowSize: int) =
        let integralImage = CalculateIntegralImage(pixels, width, height, false)
        let squaredIntegralImage = CalculateIntegralImage(pixels, width, height, true)

        let mean = Array.zeroCreate<float>(width * height)
        let std = Array.zeroCreate<float>(width * height)
        let half = windowSize / 2
        let windowArea = windowSize * windowSize

        for y = 0 to height - 1 do
            for x = 0 to width - 1 do
                let x1 = max (x - half) 0
                let y1 = max (y - half) 0
                let x2 = min (x + half) (width - 1)
                let y2 = min (y + half) (height - 1)

                let sum = integralImage.[y2, x2] - integralImage.[y1, x2] - integralImage.[y2, x1] + integralImage.[y1, x1]
                let sumSquares = squaredIntegralImage.[y2, x2] - squaredIntegralImage.[y1, x2] - squaredIntegralImage.[y2, x1] + squaredIntegralImage.[y1, x1]

                let meanValue = float sum / float windowArea
                let variance = (float sumSquares / float windowArea) - (meanValue * meanValue)
                let stdValue = Math.Sqrt variance

                let i = y * width + x
                mean.[i] <- meanValue
                std.[i] <- stdValue

        (mean, std)

    let Binarize(pixels: byte[], width: int, height: int) =
        let windowSize = int (float width * 0.02)
        let k = 0.2
        let r = 127.5

        let (mean, std) = MeanStdIntegralImage(pixels, width, height, windowSize)

        for y = 0 to height - 1 do
            for x = 0 to width - 1 do
                let i = y * width + x
                let threshold = mean.[i] * (1.0 + k * (std.[i] / r - 1.0))
                pixels.[i] <- if pixels.[i] > byte threshold then 255uy else 0uy

        pixels